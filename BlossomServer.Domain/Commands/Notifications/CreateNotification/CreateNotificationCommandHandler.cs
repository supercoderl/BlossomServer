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
                request.Message
            );

            _notificationRepository.Add(notification);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new NotificationCreatedEvent(notification.Id));
            }
        }
    }
}
