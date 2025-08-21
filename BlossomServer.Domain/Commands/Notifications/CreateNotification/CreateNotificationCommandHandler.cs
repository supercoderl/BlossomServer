using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Notification;
using MediatR;

namespace BlossomServer.Domain.Commands.Notifications.CreateNotification
{
    public sealed class CreateNotificationCommandHandler : CommandHandlerBase, IRequestHandler<CreateNotificationCommand>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            INotificationRepository notificationRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var notification = new Entities.Notification(
                request.NotificationId,
                request.UserId,
                request.Title,
                request.Message,
                request.NotificationType,
                request.Priority,
                request.ExpiresAt,
                request.ActionUrl,
                request.RelatedEntityId
            );

            _notificationRepository.Add(notification);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new NotificationCreatedEvent(
                    notification.Id,
                    notification.UserId,
                    notification,
                    notification.UserId == Guid.Empty ? "group" : "connection"
                ));
            }
        }
    }
}
