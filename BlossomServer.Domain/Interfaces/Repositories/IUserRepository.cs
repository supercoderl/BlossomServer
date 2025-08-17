using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetUserByIdentifierAsync(string identifier);

        Task<IEnumerable<User>> GetAllUsersBySQL(
            string searchTerm,
            UserRole? role,
            bool includeDeleted,
            int page,
            int pageSize,
            bool excludeBot,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );
    }
}
