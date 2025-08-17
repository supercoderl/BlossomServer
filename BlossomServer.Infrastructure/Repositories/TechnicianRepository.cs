using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using BlossomServer.SharedKernel.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class TechnicianRepository : BaseRepository<Technician, Guid>, ITechnicianRepository
    {
        private readonly string _connectionString;

        public TechnicianRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Technician>> GetAllTechniciansBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var technicians = new List<Technician>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getTechnicians", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Technicians)
            while (await reader.ReadAsync(cancellationToken))
            {
                var technician = new Technician(
                    reader.GetGuid("TechnicianId"),
                    reader.GetGuid("UserId"),
                    reader.GetString("Bio"),
                    reader.GetDouble("Rating"),
                    reader.GetInt32("YearsOfExperience")
                );

                var user = new User(
                    reader.GetGuid("UserId"),
                    string.Empty,
                    reader.GetString("FirstName"),
                    reader.GetString("LastName"),
                    reader.GetString("Email"),
                    reader.GetString("PhoneNumber"),
                    reader.GetString("AvatarUrl"),
                    null,
                    Domain.Enums.Gender.Unknow,
                    null,
                    DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                    Domain.Enums.UserRole.Technician
                );

                technician.SetUser(user);

                technicians.Add(technician);
            }

            return technicians;
        }

        public async Task<IEnumerable<object>> GetTechniciansPerformace(int page, int pageSize, string dateStart, string dateEnd, CancellationToken cancellationToken)
        {
            var technicians = new List<object>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getTechnician_performace", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@DateStart", dateStart);
            command.Parameters.AddWithValue("@DateEnd", dateEnd);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Technicians)
            while (await reader.ReadAsync(cancellationToken))
            {
                var technician = new
                {
                    Id = reader.GetGuid("Id"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    TotalBookings = reader.GetInt32("TotalBookings"),
                    TotalRevenue = reader.GetDecimal("TotalRevenue"),
                    AverageRating = reader.GetDecimal("AverageRating"),
                    UniqueCustomers = reader.GetInt32("UniqueCustomers")
                };

                technicians.Add(technician);
            }

            return technicians;
        }

        public Task<IEnumerable<object>> GetTechniciansTodayAppoinments(Guid technicianId)
        {
            throw new NotImplementedException();
        }
    }
}
