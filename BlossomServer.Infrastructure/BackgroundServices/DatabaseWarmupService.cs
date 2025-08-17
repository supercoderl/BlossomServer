using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BlossomServer.Infrastructure.BackgroundServices
{
    public sealed class DatabaseWarmupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseWarmupService> _logger;

        public DatabaseWarmupService(
            IServiceProvider serviceProvider,
            ILogger<DatabaseWarmupService> logger
        )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting database warmup...");
                var stopwatch = Stopwatch.StartNew();

                // Warm up connection pool
                await WarmupConnectionPool();

                // Warm up all critical stored procedure
                await WarmupStoredProcedure();

                // Warm up EF Core model and query compilation
                await WarmupEntityFramework();

                stopwatch.Stop();
                _logger.LogInformation($"Database warmup completed in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database warmup failed");
            }
        }

        private async Task WarmupConnectionPool()
        {
            //Open multiple connections to warm up the pool
            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await context.Database.ExecuteSqlRawAsync("SELECT 1");
                }));
            }
            await Task.WhenAll(tasks);
            _logger.LogInformation("Connection pool warmed up");
        }

        private async Task WarmupStoredProcedure()
        {
            var storedProcedures = new List<string>
            {
                // Booking
                "EXEC sp_getBookings '', 0, 1, 1",

                // Category
                "EXEC sp_getCategories '', 0, 1, 1",

                // Message
                $"EXEC sp_getMessages '', 0, 1, 1, '{Guid.Empty}'",

                // Payment
                "EXEC sp_getPayments '', 0, 1, 1",

                // Promotion
                "EXEC sp_getPromotions '', 0, 1, 1",

                // Review
                "EXEC sp_getReviews '', 0, 1, 1",

                // Service Image
                "EXEC sp_getServiceImages '', 0, 1, 1",

                // Service
                "EXEC sp_getServices '', 0, 1, 1",

                // Technician
                "EXEC sp_getTechnicians '', 0, 1, 1",

                // User
                "EXEC sp_getUsers '', 0, 1, 1",

                // Work Schedule
                "EXEC sp_getWorkSchedules '', 0, 1, 1"
            };

            var tasks = storedProcedures.Select(async sp =>
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.ExecuteSqlRawAsync(sp);
            });

            await Task.WhenAll(tasks);
            _logger.LogInformation("Stored procedures warmed up");
        }

        private async Task WarmupEntityFramework()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                // Sequential execution to avoid context conflicts
                // Add your actual query patterns here
                _ = await context.Services
                    .AsNoTracking()
                    .Take(1)
                    .ToListAsync();

                _ = await context.Users
                    .AsNoTracking()
                    .Take(1)
                    .ToListAsync();

                _ = await context.Bookings
                    .AsNoTracking()
                    .Take(1)
                    .ToListAsync();

                // Add more complex queries that match your API endpoints
                // For example, if you have joins or includes:
                _ = await context.Services
                    .AsNoTracking()
                    .Include(s => s.Category)
                    .Take(1)
                    .ToListAsync();

                _logger.LogInformation("Entity Framework queries warmed up");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Some EF warmup queries failed, but continuing...");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
