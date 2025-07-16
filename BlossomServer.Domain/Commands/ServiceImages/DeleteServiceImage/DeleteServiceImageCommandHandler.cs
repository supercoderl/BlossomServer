using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.ServiceImage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.DeleteServiceImage
{ 
    public sealed class DeleteServiceImageCommandHandler : CommandHandlerBase, IRequestHandler<DeleteServiceImageCommand>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public DeleteServiceImageCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IServiceImageRepository serviceImageRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _serviceImageRepository = serviceImageRepository;
        }

        public async Task Handle(DeleteServiceImageCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var serviceImage = await _serviceImageRepository.GetByIdAsync(request.ServiceImageId);
            
            if(serviceImage == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no any image with id {request.ServiceImageId}.",
                    ErrorCodes.ObjectNotFound
                ));

                return;
            }

            _serviceImageRepository.Remove(serviceImage);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceImageDeletedEvent(request.ServiceImageId));
            }
        }
    }
}
