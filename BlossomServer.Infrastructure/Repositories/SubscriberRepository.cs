using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class SubscriberRepository : BaseRepository<Subscriber, Guid>, ISubscriberRepository
    {
        private readonly string _connectionString;

        public SubscriberRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<Subscriber?> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            Subscriber? subscriber = null;

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getSubscriberByEmail", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                subscriber = new Subscriber(
                    reader.GetGuid("Id"),
                    reader.GetString("Email")
                );
            }

            return subscriber;
        }
    }
}
