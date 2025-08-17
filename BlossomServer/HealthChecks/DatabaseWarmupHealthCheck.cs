using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace BlossomServer.HealthChecks
{
    public class DatabaseWarmupHealthCheck : IHealthCheck
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseWarmupHealthCheck> _logger;
        private static bool _isWarmedUp = false;
        private static DateTime? _lastWarmupTime = null;
        private static readonly object _lockObject = new object();

        public DatabaseWarmupHealthCheck(IServiceProvider serviceProvider, ILogger<DatabaseWarmupHealthCheck> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var stopwatch = Stopwatch.StartNew();

                // Check database connectivity
                var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);
                if (!canConnect)
                {
                    return HealthCheckResult.Unhealthy("Cannot connect to database");
                }

                // Check if warmup is needed (every hour or on first run)
                var needsWarmup = !_isWarmedUp ||
                                _lastWarmupTime == null ||
                                DateTime.UtcNow - _lastWarmupTime > TimeSpan.FromHours(1);

                if (needsWarmup)
                {
                    // Move the warmup logic outside the lock to avoid awaiting inside the lock
                    bool performWarmup = false;

                    lock (_lockObject)
                    {
                        // Double-check pattern
                        needsWarmup = !_isWarmedUp ||
                                    _lastWarmupTime == null ||
                                    DateTime.UtcNow - _lastWarmupTime > TimeSpan.FromHours(1);

                        if (needsWarmup)
                        {
                            performWarmup = true;
                        }
                    }

                    if (performWarmup)
                    {
                        await PerformWarmup(dbContext, cancellationToken);

                        lock (_lockObject)
                        {
                            _isWarmedUp = true;
                            _lastWarmupTime = DateTime.UtcNow;
                        }
                    }
                }

                stopwatch.Stop();

                var data = new Dictionary<string, object>
                {
                    ["warmed_up"] = _isWarmedUp,
                    ["last_warmup"] = _lastWarmupTime?.ToString("yyyy-MM-dd HH:mm:ss UTC") ?? "Never",
                    ["check_duration_ms"] = stopwatch.ElapsedMilliseconds,
                    ["database_responsive"] = true
                };

                return HealthCheckResult.Healthy("Database is warmed up and responsive", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database warmup health check failed");

                var data = new Dictionary<string, object>
                {
                    ["error"] = ex.Message,
                    ["warmed_up"] = _isWarmedUp,
                    ["last_warmup"] = _lastWarmupTime?.ToString("yyyy-MM-dd HH:mm:ss UTC") ?? "Never"
                };

                return HealthCheckResult.Degraded("Database warmup check failed", ex, data);
            }
        }

        private async Task PerformWarmup(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Performing database warmup via health check...");

            var warmupTasks = new List<Task>
        {
            // Connection pool warmup
            context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken),
            
            // Critical stored procedures warmup
            context.Database.ExecuteSqlRawAsync("EXEC sp_getServices '', 0, 1, 1", cancellationToken),
            context.Database.ExecuteSqlRawAsync("EXEC sp_getUsers '', 1, 1", cancellationToken),
            context.Database.ExecuteSqlRawAsync("EXEC sp_getBookings '', GETDATE(), GETDATE(), 1, 1", cancellationToken),
            
            // EF Core model warmup
            context.Services.Take(1).LoadAsync(cancellationToken),
            context.Users.Take(1).LoadAsync(cancellationToken),
            context.Bookings.Take(1).LoadAsync(cancellationToken),
        };

            await Task.WhenAll(warmupTasks);
            _logger.LogInformation("Database warmup completed successfully");
        }

        // Static method to mark warmup as completed (called from DatabaseWarmupService)
        public static void MarkAsWarmedUp()
        {
            lock (_lockObject)
            {
                _isWarmedUp = true;
                _lastWarmupTime = DateTime.UtcNow;
            }
        }

        // Static method to force re-warmup
        public static void RequireWarmup()
        {
            lock (_lockObject)
            {
                _isWarmedUp = false;
            }
        }
    }
}
