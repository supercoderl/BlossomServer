using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class ServiceImageRepository : BaseRepository<ServiceImage, Guid>, IServiceImageRepository
    {
        private readonly string _connectionString;

        public ServiceImageRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<ServiceImage>> GetAllServiceImagesBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var serviceImages = new List<ServiceImage>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getServiceImages", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Services)
            while (await reader.ReadAsync(cancellationToken))
            {
                var serviceImage = new ServiceImage(
                    reader.GetGuid("Id"),
                    reader.GetString("ImageName"),
                    reader.GetString("ImageUrl"),
                    reader.GetGuid("ServiceId"),
                    reader.IsDBNull("Description") ? null : reader.GetString("Description")
                );

                serviceImage.SetCreatedAt(reader.GetDateTime("CreatedAt"));

                var service = new Service(
                    reader.GetGuid("ServiceId"),
                    reader.GetString("Name"),
                    reader.IsDBNull("ServiceDescription") ? null : reader.GetString("ServiceDescription"),
                    reader.IsDBNull("CategoryId") ? null : reader.GetGuid("CategoryId"),
                    reader.IsDBNull("Price") ? null : reader.GetDecimal("Price"),
                    reader.IsDBNull("DurationMinutes") ? null : reader.GetInt32("DurationMinutes"),
                    reader.IsDBNull("RepresentativeImage") ? null : reader.GetString("RepresentativeImage")
                );

                service.SetCreatedAt(reader.GetDateTime("ServiceCreatedAt"));
                service.SetUpdatedAt(reader.IsDBNull("UpdatedAt") ? null : reader.GetDateTime("UpdatedAt"));

                serviceImage.SetService(service);

                serviceImages.Add(serviceImage);
            }

            return serviceImages;
        }
    }
}
