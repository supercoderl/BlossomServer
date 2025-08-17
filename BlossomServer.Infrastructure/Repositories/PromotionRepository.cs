using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class PromotionRepository : BaseRepository<Promotion, Guid>, IPromotionRepository
    {
        private readonly string _connectionString;

        public PromotionRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<Promotion?> CheckByCode(string code)
        {
            return await DbSet.SingleOrDefaultAsync(c => c.Code == code);
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var promotions = new List<Promotion>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getPromotions", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Promotions)
            while (await reader.ReadAsync(cancellationToken))
            {
                var promotion = new Promotion(
                    reader.GetGuid("Id"),
                    reader.GetString("Code"),
                    reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                    (DiscountType)Enum.Parse(typeof(DiscountType), reader.GetString("DiscountType")),
                    reader.GetDecimal("DiscountValue"),
                    reader.GetDecimal("MinimumSpend"),
                    reader.GetDateTime("StartDate"),
                    reader.GetDateTime("EndDate"),
                    reader.GetInt32("MaxUsage"),
                    reader.GetInt32("CurrentUsage"),
                    reader.GetBoolean("IsActive")
                );

                promotions.Add(promotion);
            }

            return promotions;
        }
    }
}
