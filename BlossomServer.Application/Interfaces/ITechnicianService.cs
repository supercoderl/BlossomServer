using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Technicians;

namespace BlossomServer.Application.Interfaces
{
    public interface ITechnicianService
    {
        public Task<TechnicianViewModel?> GetTechnicianByTechnicianIdAsync(Guid technicianId);

        public Task<PagedResult<TechnicianViewModel>> GetAllTechniciansAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task UpdateTechnicianAsync(UpdateTechnicianViewModel technician);
        public Task DeleteTechnicianAsync(Guid technicianId);
    }
}
