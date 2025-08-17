using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Shared.Events.Booking;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Consumers
{
    public sealed class BookingCreatedEventConsumer : IConsumer<BookingCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BookingCreatedEventConsumer> _logger;

        public BookingCreatedEventConsumer(IPublishEndpoint publishEndpoint, ILogger<BookingCreatedEventConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BookingCreatedEvent> context)
        {
            var emailCommand = new SendMailCommand(
                context.Message.CustomerEmail,
                "Booking Confirmation",
                $@"
                    Dear {context.Message.CustomerName},

                    Thank you for your booking!

                    This is a confirmation that your reservation has been successfully completed. Below are your booking details:

                    ------------------------------------------------------------
                    📌 Booking ID: [#{context.Message.AggregateId.ToString().Substring(0, 6)}]
                    📍 Location: [8c wells place SO50 5PP, Eastleigh, UK]
                    📅 Date: [{context.Message.ScheduleTime.ToString("dddd, MMMM dd, yyyy")}]
                    ⏰ Time: [{context.Message.ScheduleTime.ToString("h:mm tt") + " Check-in"}]
                    👤 Guest Name: [{context.Message.CustomerName}]
                    📞 Contact: [{context.Message.CustomerPhone}]
                    💳 Payment Status: [Pending]
                    ------------------------------------------------------------

                    If you have any questions or need to make changes to your booking, please contact us at [blossom_nails2018@outlook.com/+44 23 8061 3526].

                    We look forward to welcoming you!

                    Best regards,  
                    Blossom Nails 
                    https://blossom-pi.vercel.app
                ",
                false
            );

            await _publishEndpoint.Publish(emailCommand);

            _logger.LogInformation("Email queued for booking {BookingId}", context.Message.AggregateId);
        }
    }
}
