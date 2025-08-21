using BlossomServer.Domain.Commands.BookingDetails.CreateBookingDetail;
using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Domain.Commands.Notifications.CreateNotification;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Shared.Events.Booking;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BlossomServer.Domain.Consumers
{
    public sealed class BookingCreatedEventConsumer : IConsumer<BookingCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BookingCreatedEventConsumer> _logger;
        private readonly IMediatorHandler _bus;

        public BookingCreatedEventConsumer(
            IPublishEndpoint publishEndpoint, 
            ILogger<BookingCreatedEventConsumer> logger,
            IMediatorHandler bus
        )
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _bus = bus;
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

            var bookingDetailCommand = new CreateBookingDetailCommand(
                Guid.NewGuid(),
                context.Message.AggregateId,
                context.Message.ServiceId,
                context.Message.ServiceOptionId,
                context.Message.Quantity,
                context.Message.Price
            );

            var adminNotificationCommand = new CreateNotificationCommand(
                Guid.NewGuid(),
                Guid.Empty, // Admin notification
                "Booking Arrived",
                "You have a new booking!",
                Enums.NotificationType.NewBooking,
                0,
                null,
                null,
                null
            );

            CreateNotificationCommand? technicianNotificationCommand = null;
            if (context.Message.ReceiverId.HasValue)
            {
                technicianNotificationCommand = new CreateNotificationCommand(
                    Guid.NewGuid(),
                    context.Message.ReceiverId.Value,
                    "You Have a New Appointment",
                    $"A customer has booked at [{context.Message.ScheduleTime.ToString("yyyy/MM/dd - HH:mm:ss")}]. Please confirm.",
                    Enums.NotificationType.NewBooking,
                    0,
                    null,
                    null,
                    null
                );
            }

            await _publishEndpoint.Publish(emailCommand);
            await _bus.SendCommandAsync(bookingDetailCommand);
            await _publishEndpoint.Publish(adminNotificationCommand);

            if (technicianNotificationCommand != null)
            {
                await _publishEndpoint.Publish(technicianNotificationCommand);
            }

            _logger.LogInformation("Email queued for booking {BookingId}", context.Message.AggregateId);
        }
    }
}
