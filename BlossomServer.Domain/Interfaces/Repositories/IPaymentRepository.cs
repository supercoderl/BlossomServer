using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IRepository<Payment, Guid>
    {
        Task<IEnumerable<Payment>> GetAllPaymentsBySQL(
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
