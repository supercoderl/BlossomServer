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
    public sealed class ServiceRepository : BaseRepository<Service, Guid>, IServiceRepository
    {
        private readonly string _connectionString;

        public ServiceRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Service>> GetAllServicesBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var services = new List<Service>();
            var serviceOptions = new List<ServiceOption>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getServices", connection)
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
                var service = new Service(
                    reader.GetGuid("Id"),
                    reader.GetString("Name"),
                    reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    reader.GetGuid("CategoryId"),
                    reader.GetDecimal("Price"),
                    reader.GetInt32("DurationMinutes"),
                    reader.IsDBNull("RepresentativeImage") ? null : reader.GetString("RepresentativeImage")
                );

                service.SetCreatedAt(reader.GetDateTime("UpdatedAt"));
                service.SetUpdatedAt(reader.IsDBNull("DeletedAt") ? null : reader.GetDateTime("DeletedAt"));

                services.Add(service);
            }

            // Move to second result set (ServiceOptions)
            await reader.NextResultAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var serviceOption = new ServiceOption(
                    reader.GetGuid("Id"),
                    reader.GetGuid("ServiceId"),
                    reader.GetString("VariantName"),
                    reader.GetDecimal("PriceFrom"),
                    reader.IsDBNull("PriceTo") ? null : reader.GetDecimal("PriceTo"),
                    reader.IsDBNull("DurationMinutes") ? null : reader.GetInt32("DurationMinutes")
                );

                serviceOptions.Add(serviceOption);
            }

            // Combine the results
            var serviceOptionsLookup = serviceOptions.ToLookup(so => so.ServiceId);

            foreach (var s in services)
            {
                var options = serviceOptionsLookup[s.Id];
                s.SetServiceOptions(options.ToList());
            }

            return services;
        }

        public async Task<decimal> GetAverageServiceValue(string dateStart, string dateEnd, CancellationToken cancellationToken = default)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getAverageService_value", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@DateStart", dateStart);
            command.Parameters.AddWithValue("@DateEnd", dateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Services)
            while (await reader.ReadAsync(cancellationToken))
            {
                return reader.GetDecimal("AverageServiceValue");
            }

            return 0;
        }

        public async Task<IEnumerable<object>> GetServicesPopularityRanking(string currentDateStart, string currentDateEnd, string previousDateStart, string previousDateEnd, CancellationToken cancellationToken)
        {
            var services = new List<object>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getService_popularity_ranking", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CurrentPeriodStart", currentDateStart);
            command.Parameters.AddWithValue("@CurrentPeriodEnd", currentDateEnd);
            command.Parameters.AddWithValue("@ComparisonPeriodStart", previousDateStart);
            command.Parameters.AddWithValue("@ComparisonPeriodEnd", previousDateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Services)
            while (await reader.ReadAsync(cancellationToken))
            {
                var service = new
                {
                    ServiceName = reader.GetString("ServiceName"),
                    Price = reader.GetDecimal("Price"),
                    DurationMinutes = reader.GetInt32("DurationMinutes"),
                    BookingCount = reader.GetInt32("BookingCount"),
                    TotalRevenue = reader.GetDecimal("TotalRevenue"),
                    AverageRating = reader.IsDBNull("AverageRating") ? 0 : reader.GetDecimal("AverageRating"),
                    ServiceType = reader.GetString("ServiceType"),
                    SourceId = reader.GetGuid("SourceId"),
                    ReviewCount = reader.GetInt32("ReviewCount"),
                    AverageRevenuePerBooking = reader.GetDecimal("AverageRevenuePerBooking"),
                    PreviousBookingCount = reader.GetInt32("PreviousBookingCount"),
                    PreviousTotalRevenue = reader.GetDecimal("PreviousTotalRevenue"),
                    PreviousAverageRating = reader.IsDBNull("PreviousAverageRating") ? 0 : reader.GetDecimal("PreviousAverageRating"),
                    BookingCountChangePercent = reader.GetDecimal("BookingCountChangePercent"),
                    RevenueChangePercent = reader.GetDecimal("RevenueChangePercent"),
                    RatingChangePoints = reader.IsDBNull("RatingChangePoints") ? 0 : reader.GetDecimal("RatingChangePoints"),
                    BookingCountChange = reader.GetInt32("BookingCountChange"),
                    RevenueChange = reader.GetDecimal("RevenueChange"),
                    TrendStatus = reader.GetString("TrendStatus")
                };

                services.Add(service);
            }

            return services;
        }
    }
}
