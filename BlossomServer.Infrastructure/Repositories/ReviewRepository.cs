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
    public sealed class ReviewRepository : BaseRepository<Review, Guid>, IReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Review>> GetAllReviewsBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var reviews = new List<Review>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getReviews", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Reviews)
            while (await reader.ReadAsync(cancellationToken))
            {
                var review = new Review(
                    reader.GetGuid("Id"),
                    reader.GetGuid("BookingId"),
                    reader.GetGuid("CustomerId"),
                    reader.GetGuid("TechnicianId"),
                    reader.GetInt32("Rating"),
                    reader.GetString("Comment")
                );

                reviews.Add(review);
            }

            return reviews;
        }
    }
}
