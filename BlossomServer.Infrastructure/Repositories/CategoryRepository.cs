using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Text;
using System.Threading;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var categories = new List<Category>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getCategories", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Categories)
            while (await reader.ReadAsync(cancellationToken))
            {
                var category = new Category(
                    reader.GetGuid("Id"),
                    reader.GetString("Name"),
                    reader.GetBoolean("IsActive"),
                    reader.GetString("Icon"),
                    reader.GetString("Url"),
                    reader.GetInt32("Priority")
                );

                category.SetCreatedAt(reader.GetDateTime("CreatedAt"));
                category.SetUpdatedAt(reader.IsDBNull("UpdatedAt") ? null : reader.GetDateTime("UpdatedAt"));

                categories.Add(category);
            }

            return categories;
        }

        public async Task<List<object>> GetCategoriesWithServicesDetailSQL(CancellationToken cancellationToken = default)
        {
            var categories = new List<object>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getCategoriesWithServicesDetailed", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Categories)
            while (await reader.ReadAsync(cancellationToken))
            {
                var category = new
                {
                    CategoryId = reader.GetGuid("CategoryId"),
                    CategoryName = reader.GetString("CategoryName"),
                    CategoryIsActive = reader.GetBoolean("CategoryIsActive"),
                    ServiceCount = reader.GetInt32("ServiceCount"),
                    ServiceOptionCount = reader.GetInt32("ServiceOptionCount")
                };

                categories.Add(category);
            }

            return categories;
        }
    }
}
