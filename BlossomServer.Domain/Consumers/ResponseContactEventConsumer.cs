using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Shared.Events.ContactResponse;
using BlossomServer.Shared.Events.Subscriber;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Consumers
{
    public sealed class ResponseContactEventConsumer : IConsumer<ContactResponseCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ResponseContactEventConsumer> _logger;

        public ResponseContactEventConsumer(IPublishEndpoint publishEndpoint, ILogger<ResponseContactEventConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ContactResponseCreatedEvent> context)
        {
            var emailCommand = new SendMailCommand(
                context.Message.Email,
                "Response your contact!",
                context.Message.ResponseText,
                false
            );

            await _publishEndpoint.Publish(emailCommand);

            _logger.LogInformation("Email queued for contact response {ContactResponseId}", context.Message.AggregateId);
        }
    }
}
