using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IServiceService
    {
        public Task<ServiceViewModel?> GetServiceByServiceIdAsync(Guid serviceId);

        public Task<PagedResult<ServiceViewModel>> GetAllServicesAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task<Guid> CreateServiceAsync(CreateServiceViewModel service);
        public Task UpdateServiceAsync(UpdateServiceViewModel service);
        public Task DeleteServiceAsync(Guid serviceId);
    }
}
