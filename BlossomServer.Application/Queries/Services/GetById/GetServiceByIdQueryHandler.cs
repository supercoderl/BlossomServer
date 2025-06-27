using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.Services.GetById
{
    public sealed class GetServiceByIdQueryHandler :
        IRequestHandler<GetServiceByIdQuery, ServiceViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly IServiceRepository _serviceRepository;

        public GetServiceByIdQueryHandler(IServiceRepository serviceRepository, IMediatorHandler bus)
        {
            _serviceRepository = serviceRepository;
            _bus = bus;
        }

        public async Task<ServiceViewModel?> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByIdAsync(request.Id);

            if (service is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetServiceByIdQuery),
                        $"Service with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return ServiceViewModel.FromService(service);
        }
    }
}
