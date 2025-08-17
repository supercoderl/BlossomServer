using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Consumers
{
    public sealed class EmailConsumer : IConsumer<SendMailCommand>
    {
        private readonly IMediatorHandler _bus;
        private readonly ILogger<EmailConsumer> _logger;

        public EmailConsumer(IMediatorHandler bus, ILogger<EmailConsumer> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendMailCommand> context)
        {
            try
            {
                _logger.LogInformation("Processing email for {To} with content {Content}",
                    context.Message.To, context.Message.Content);

                await _bus.SendCommandAsync(new SendMailCommand(
                    context.Message.To,
                    context.Message.Subject,
                    context.Message.Content,
                    false
                ));

                _logger.LogInformation("Email sent successfully to {To}", context.Message.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", context.Message.To);
                throw; // This will trigger retry mechanism
            }
        }
    }
}
