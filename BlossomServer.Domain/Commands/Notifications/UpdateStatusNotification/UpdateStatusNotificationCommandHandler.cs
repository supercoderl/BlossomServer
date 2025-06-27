using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Notification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Notifications.UpdateStatusNotification
{
    public sealed class UpdateStatusNotificationCommandHandler : CommandHandlerBase, IRequestHandler<UpdateStatusNotificationCommand>
    {
        private readonly INotificationRepository _notificationRepository;

        public UpdateStatusNotificationCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            INotificationRepository notificationRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(UpdateStatusNotificationCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;

            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);

            if (notification == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no notification with ID {request.NotificationId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            notification.SetIsRead(true);

            _notificationRepository.Update(notification);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new NotificationUpdatedEvent(notification.Id));
            }
        }
    }
}
