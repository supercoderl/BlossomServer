using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class WorkScheduleRepository : BaseRepository<WorkSchedule, Guid>, IWorkScheduleRepository
    {
        private readonly string _connectionString;

        public WorkScheduleRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<WorkSchedule>> GetAllWorkSchedulesBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var workSchedules = new List<WorkSchedule>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getWorkSchedules", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (WorkSchedules)
            while (await reader.ReadAsync(cancellationToken))
            {
                var workSchedule = new WorkSchedule(
                    reader.GetGuid("Id"),
                    reader.GetGuid("TechnicianId"),
                    DateOnly.FromDateTime(reader.GetDateTime("WorkDate")),
                    TimeOnly.FromDateTime(reader.GetDateTime("StartTime")),
                    TimeOnly.FromDateTime(reader.GetDateTime("EndTime")),
                    reader.GetBoolean("IsDayOff")
                );

                workSchedules.Add(workSchedule);
            }

            return workSchedules;
        }
    }
}
