using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface ITechnicianRepository : IRepository<Technician, Guid>
    {
        Task<IEnumerable<Technician>> GetAllTechniciansBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<object>> GetTechniciansPerformace(
            int page,
            int pageSize,
            string dateStart,
            string dateEnd,
            CancellationToken cancellationToken
        );
        Task<IEnumerable<object>> GetTechniciansTodayAppoinments(Guid technicianId);
    }
}
