using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Shared.Events.Subscriber;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BlossomServer.Domain.Consumers
{
    public sealed class UserSubscribedEventConsumer : IConsumer<SubscribeEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<UserSubscribedEventConsumer> _logger;

        public UserSubscribedEventConsumer(IPublishEndpoint publishEndpoint, ILogger<UserSubscribedEventConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubscribeEvent> context)
        {
            var emailCommand = new SendMailCommand(
                context.Message.Email,
                "🎉 Thank You for Subscribing!",
                """
                    Hi there,

                    Thank you for subscribing to our newsletter! 🎉  
                    You're now part of our inner circle, which means you'll be the first to hear about:

                    - Exclusive deals and special offers  
                    - The latest product updates and launches  
                    - Tips, news, and insights curated just for you

                    We're excited to have you with us!

                    📩 Be sure to keep an eye on your inbox — we’ve got some great content coming your way.

                    If you ever have any questions, feel free to reply to this email — we’re here to help!

                    Cheers,  
                    Blossom Nails Team
                 """,
                false
            );

            await _publishEndpoint.Publish(emailCommand);

            _logger.LogInformation("Email queued for subscriber {SubscriberId}", context.Message.AggregateId);
        }
    }
}
