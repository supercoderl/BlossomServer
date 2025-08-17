using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IBlogRepository : IRepository<Blog, Guid>
    {
        Task<IEnumerable<Blog>> GetAllBlogsBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            bool isPublished,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );
    }
}
