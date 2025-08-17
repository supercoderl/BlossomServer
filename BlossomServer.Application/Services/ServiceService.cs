using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Services.GetAll;
using BlossomServer.Application.Queries.Services.GetAllBySQL;
using BlossomServer.Application.Queries.Services.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Services.CreateService;
using BlossomServer.Domain.Commands.Services.DeleteService;
using BlossomServer.Domain.Commands.Services.UpdateService;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IMediatorHandler _bus;

        public ServiceService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> CreateServiceAsync(CreateServiceViewModel service)
        {
            var serviceId = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateServiceCommand(
                serviceId,
                service.Name,
                service.Description,
                service.CategoryId,
                service.Price,
                service.DurationInMinutes,
                service.RepresentativeImage
            ));

            return serviceId;
        }

        public async Task DeleteServiceAsync(Guid serviceId)
        {
            await _bus.SendCommandAsync(new DeleteServiceCommand(serviceId));
        }

        public async Task<PagedResult<ServiceViewModel>> GetAllServicesAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllServicesQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<PagedResult<ServiceViewModel>> GetAllServicesBySQLAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllServicesBySQLQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<ServiceViewModel?> GetServiceByServiceIdAsync(Guid serviceId)
        {
            return await _bus.QueryAsync(new GetServiceByIdQuery(serviceId));
        }

        public async Task UpdateServiceAsync(UpdateServiceViewModel service)
        {
            await _bus.SendCommandAsync(new UpdateServiceCommand(
                service.ServiceId,
                service.Name,
                service.Description,
                service.CategoryId,
                service.Price,
                service.DurationInMinutes,
                service.RepresentativeImage
            ));
        }
    }
}
