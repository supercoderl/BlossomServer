using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion, Guid>
    {
        Task<Promotion?> CheckByCode(string code);

        Task<IEnumerable<Promotion>> GetAllPromotionsBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );
    }
}
