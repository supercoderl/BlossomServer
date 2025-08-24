using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using BlossomServer.SharedKernel.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<User>> GetAllUsersBySQL(string searchTerm, UserRole? role, bool includeDeleted, int page, int pageSize, bool exludeBot, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var users = new List<User>();
            var technicians = new List<Technician>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getUsers", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@ExcludeBot", exludeBot);
            command.Parameters.AddWithValue("@Role", role);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Users)
            while (await reader.ReadAsync(cancellationToken))
            {
                var user = new User(
                    reader.GetGuid("Id"),
                    string.Empty,
                    reader.GetString("FirstName"),
                    reader.GetString("LastName"),
                    reader.GetString("Email"),
                    reader.GetString("PhoneNumber"),
                    reader.GetString("AvatarUrl"),
                    reader.IsDBNull("CoverPhotoUrl") ? null : reader.GetString("CoverPhotoUrl"),
                    (Gender)reader.GetInt32("Gender"),
                    reader.IsDBNull("Website") ? null : reader.GetString("Website"),
                    DateOnly.FromDateTime(reader.GetDateTime("DateOfBirth")),
                    (UserRole)reader.GetInt32("Role"),
                    (UserStatus)reader.GetInt32("Status")
                );

                user.SetLastLoggedIn(reader.IsDBNull("LastLoggedinDate") ? null : reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal("LastLoggedinDate")));

                users.Add(user);
            }

            // Move to second result set (Technicians)
            await reader.NextResultAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var technician = new Technician(
                    reader.GetGuid("Id"),
                    reader.GetGuid("UserId"),
                    reader.GetString("Bio"),
                    reader.GetDouble("Rating"),
                    reader.GetInt32("YearsOfExperience")
                );

                technicians.Add(technician);
            }

            foreach (var u in users)
            {
                var technician = technicians.Find(t => t.UserId == u.Id);
                u.SetTechnician(technician);
            }

            return users;
        }

        public async Task<User?> GetUserByIdentifierAsync(string identifier)
        {
            var parameter = new SqlParameter("@NormalizedEmail", TextHelper.NomalizeGmail(identifier));

            var result = await DbSet
                .FromSqlRaw("EXEC sp_getUserByIdentifier @NormalizedEmail", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }
    }
}
