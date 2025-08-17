using BlossomServer.Application.Interfaces;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class AuditLogRepository : BaseRepository<AuditLog, Guid>, IAuditLogRepository, IContextInfoService
    {
        private readonly string _connectionString;
        private readonly ApplicationDbContext _dbContext;

        public AuditLogRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new SqlNullValueException("An error occurred when fetching sql connection string");
            _dbContext = context;
        }

        public async Task SetContextAsync(string? userId, string? ipAddress = null, string? userAgent = null, string? sessionId = null, CancellationToken cancellationToken = default)
        {
            var contextInfo = new { UserId = userId, IpAddress = ipAddress, UserAgent = userAgent, SessionId = sessionId };
            var contextJson = JsonSerializer.Serialize(contextInfo);

            var contextBytes = Encoding.UTF8.GetBytes(contextJson);

            if (contextBytes.Length > 128)
            {
                contextJson = JsonSerializer.Serialize(new { UserId = userId ?? "" });
                contextBytes = Encoding.UTF8.GetBytes(contextJson);

                if (contextBytes.Length > 128)
                {
                    Array.Resize(ref contextBytes, 128);
                }
            }

            var finalBytes = new byte[128];
            Array.Copy(contextBytes, 0, finalBytes, 0, Math.Min(contextBytes.Length, 128));

            // Use Entity Framework's ExecuteSqlRaw to ensure same connection
            var contextParam = new SqlParameter("@ContextInfo", SqlDbType.Binary, 128)
            {
                Value = finalBytes
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "SET CONTEXT_INFO @ContextInfo",
                new[] { contextParam },
                cancellationToken);
        }

        public async Task ClearContextAsync(CancellationToken cancellationToken = default)
        {
            var nullBytes = new byte[128];
            var contextParam = new SqlParameter("@ContextInfo", SqlDbType.Binary, 128)
            {
                Value = nullBytes
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "SET CONTEXT_INFO @ContextInfo",
                new[] { contextParam },
                cancellationToken);
        }
    }
}