using BlossomServer.Domain.Commands.Notifications.CreateNotification;
using BlossomServer.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BlossomServer.Domain.Consumers
{
    public sealed class NotificationRequiredEventConsumer : IConsumer<CreateNotificationCommand>
    {
        private readonly IMediatorHandler _bus;
        private readonly ILogger<NotificationRequiredEventConsumer> _logger;

        public NotificationRequiredEventConsumer(IMediatorHandler bus, ILogger<NotificationRequiredEventConsumer> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateNotificationCommand> context)
        {
            try
            {
                await _bus.SendCommandAsync(context.Message);

                _logger.LogInformation("Notification created for with id; {id}", context.Message.NotificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed create notification for with id; {id}", context.Message.NotificationId);
                throw; // This will trigger retry mechanism
            }
        }
    }
}
