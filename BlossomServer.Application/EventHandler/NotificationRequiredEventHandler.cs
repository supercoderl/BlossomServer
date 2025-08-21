using BlossomServer.Application.Interfaces;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Shared.Events.Notification;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlossomServer.Application.EventHandler
{
    public sealed class NotificationRequiredEventHandler :
        INotificationHandler<NotificationCreatedEvent>
    {
        private readonly ISignalRService _signalRService;
        private readonly ILogger<NotificationRequiredEventHandler> _logger;
        private readonly IMediatorHandler _bus;

        public NotificationRequiredEventHandler(
            ISignalRService signalRService,
            ILogger<NotificationRequiredEventHandler> logger,
            IMediatorHandler bus
        )
        {
            _signalRService = signalRService;
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _signalRService.SendData(
                    "system",
                    notification.Notification,
                    notification.Type,
                    notification.Type == "group" ? "administrators" : null,
                    notification.ReceiverId.ToString()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send socket notification");
                // Don't rethrow - socket failure shouldn't stop database save
            }
        }
    }
}
