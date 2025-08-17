using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using BlossomServer.SharedKernel.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class BlogRepository : BaseRepository<Blog, Guid>, IBlogRepository
    {
        private readonly string _connectionString;

        public BlogRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, bool isPublished, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var blogs = new List<Blog>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getBlogs", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@IsPublished", isPublished);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Blogs)
            while (await reader.ReadAsync(cancellationToken))
            {
                var blog = new Blog(
                    reader.GetGuid("Id"),
                    reader.GetString("Title"),
                    reader.GetString("Slug"),
                    reader.GetString("Content"),
                    reader.GetGuid("AuthorId"),
                    reader.IsDBNull("Tags") ? string.Empty : reader.GetString("Tags"),
                    reader.GetDateTime("PublishedAt"),
                    reader.GetBoolean("IsPublished"),
                    reader.GetString("ThumbnailUrl")
                );

                blog.SetUpdatedAt(reader.IsDBNull("UpdatedAt") ? null : reader.GetDateTime("UpdatedAt"));
                blog.SetViewsCount(reader.GetInt32("ViewsCount"));
                blog.SetCommentsCount(reader.GetInt32("CommentsCount"));

                User user = new User(
                        reader.GetGuid("AuthorId"),
                        string.Empty,
                        reader.IsDBNull("FirstName") ? string.Empty : reader.GetString("FirstName"),
                        reader.IsDBNull("LastName") ? string.Empty : reader.GetString("LastName"),
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        null,
                        Gender.Unknow,
                        null,
                        DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                        UserRole.Customer
                    );

                blog.SetUser(user);

                blogs.Add(blog);
            }

            return blogs;
        }
    }
}
