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

namespace BlossomServer.Domain.Commands.Notifications.DeleteNotification
{
    public sealed class DeleteNotificationCommandHandler : CommandHandlerBase, IRequestHandler<DeleteNotificationCommand>
    {
        private readonly INotificationRepository _notificationRepository;

        public DeleteNotificationCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            INotificationRepository notificationRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);

            if(notification == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Notification not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            _notificationRepository.Remove(notification);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new NotificationDeletedEvent(request.NotificationId));
            }
        }
    }
}
