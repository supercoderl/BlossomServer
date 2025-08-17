using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Shared.Events.EmailReminder;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BlossomServer.Domain.Consumers
{
    public class SendEmailReminderConsumer : IConsumer<SendEmailReminderEvent>
    {
        private readonly IMediatorHandler _bus;
        private readonly ILogger<SendEmailReminderConsumer> _logger;

        public SendEmailReminderConsumer(
            IMediatorHandler bus,
            ILogger<SendEmailReminderConsumer> logger
        )
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendEmailReminderEvent> context)
        {
            var message = context.Message;

            try
            {
                var emailContent = BuildEmailContent(message);

                await _bus.SendCommandAsync(new SendMailCommand(
                    message.RecipientEmail,
                    message.Subject,
                    emailContent,
                    true
                ));

                _logger.LogInformation("Email sent successfully to {RecipientEmail} for {ReminderType}",
                    message.RecipientEmail, message.ReminderType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {RecipientEmail} for {ReminderType}",
                    message.RecipientEmail, message.ReminderType);
                throw;
            }
        }

        private string BuildEmailContent(SendEmailReminderEvent message)
        {
            var recipientTitle = (RecipientType)message.RecipientType == RecipientType.Employee ? "Dear Employee" : "Dear Valued Customer";
            var targetDateFormatted = message.TargetDate.ToString("MMMM dd, yyyy");

            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2c3e50;'>{recipientTitle} {message.RecipientName},</h2>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-left: 4px solid #007bff; margin: 20px 0;'>
                        <p style='margin: 0; font-weight: bold;'>Reminder: {message.Subject}</p>
                    </div>
                    
                    <p>{message.Message}</p>
                    
                    <p><strong>Date:</strong> {targetDateFormatted}</p>
                    
                    <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;'>
                        <p>If you have any questions, please don't hesitate to contact us.</p>
                        <p style='color: #666; font-size: 14px;'>
                            Best regards,<br/>
                            Your Team
                        </p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
