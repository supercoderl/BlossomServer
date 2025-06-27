using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Services.DeleteService
{
    public sealed class DeleteServiceCommandHandler : CommandHandlerBase, IRequestHandler<DeleteServiceCommand>
    {
        private readonly IServiceRepository _serviceRepository;

        public DeleteServiceCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IServiceRepository serviceRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;

            var service = await _serviceRepository.GetByIdAsync(request.ServiceId);

            if (service == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Service not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            _serviceRepository.Remove(service);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceDeletedEvent(service.Id));
            }
        }
    }
}
