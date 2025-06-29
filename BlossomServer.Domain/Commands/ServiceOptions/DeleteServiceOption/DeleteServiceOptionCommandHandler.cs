using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.ServiceOption;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.DeleteServiceOption
{
    public sealed class DeleteServiceOptionCommandHandler : CommandHandlerBase, IRequestHandler<DeleteServiceOptionCommand>
    {
        private readonly IServiceOptionRepository _serviceOptionRepository;

        public DeleteServiceOptionCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IServiceOptionRepository serviceOptionRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _serviceOptionRepository = serviceOptionRepository;
        }

        public async Task Handle(DeleteServiceOptionCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var serviceOption = await _serviceOptionRepository.GetByIdAsync(request.ServiceOptionId);

            if(serviceOption == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no any service option with id {request.ServiceOptionId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            _serviceOptionRepository.Remove(serviceOption);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceOptionDeletedEvent(request.ServiceOptionId));
            }
        }
    }
}
