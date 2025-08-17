using BlossomServer.Domain.Commands.EmailReminders.AddReminder;
using BlossomServer.Domain.Commands.EmailReminders.UpdateStatus;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.BackgroundServices;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Shared.Events.EmailReminder;
using BlossomServer.SharedKernel.Utils;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BlossomServer.Infrastructure.BackgroundServices
{
    public class EmailReminderBackgroundService : IEmailReminderBackgroundService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailReminderRepository _emailReminderRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMediatorHandler _bus;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EmailReminderBackgroundService> _logger;

        public EmailReminderBackgroundService(
            IBookingRepository bookingRepository,
            IEmailReminderRepository emailReminderRepository,
            IBackgroundJobClient backgroundJobClient,
            IMediatorHandler bus,
            IPublishEndpoint publishEndpoint,
            ILogger<EmailReminderBackgroundService> logger)
        {
            _bookingRepository = bookingRepository;
            _emailReminderRepository = emailReminderRepository;
            _backgroundJobClient = backgroundJobClient;
            _bus = bus;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task ProcessEmailRemindersAsync()
        {
            try
            {
                _logger.LogInformation("Starting email reminder processing at {DateTime}", DateTime.UtcNow);

                // Process Booking Reminders
                await ProcessBookingRemindersAsync();

                _logger.LogInformation("Email reminder processing completed at {DateTime}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email reminders");
                throw;
            }
        }

        private async Task ProcessBookingRemindersAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingsIncommingBySQL(1, 20);

            foreach (var booking in bookings)
            {
                if (booking.TechnicianId.HasValue && booking.Technician?.User != null)
                {
                    await CreateReminderIfNeeded(
                        booking.TechnicianId.Value,
                        booking.Technician.User.Email,
                        booking.Technician.User.FullName,
                        RecipientType.Employee,
                        EmailReminderType.FollowUpReminder,
                        booking.ScheduleTime,
                        "Appointment Coming Soon",
                        "You have an upcoming appointment scheduled soon. Please check your calendar.");
                }

                // Customer reminder (only if email exists)
                var customerEmail = booking.Customer?.Email ?? booking.GuestEmail;
                if (!string.IsNullOrWhiteSpace(customerEmail))
                {
                    await CreateReminderIfNeeded(
                        booking.CustomerId ?? Guid.Empty,
                        customerEmail,
                        booking.Customer?.FullName ?? booking.GuestName ?? string.Empty,
                        RecipientType.Customer,
                        EmailReminderType.FollowUpReminder,
                        booking.ScheduleTime,
                        "Appointment Coming Soon",
                        "You have an upcoming appointment scheduled soon. Please check your calendar."
                    );
                }
            }
        }

        private async Task CreateReminderIfNeeded(
            Guid entityId,
            string email,
            string name,
            RecipientType recipientType,
            EmailReminderType reminderType,
            DateTime targetDate,
            string subject,
            string message)
        {
            var reminderDate = targetDate.Date.AddDays(-1); // 1 day before

            // Check if reminder already exists
            var existingReminder = await _emailReminderRepository.GetByEntityAndTypeAsync(
                entityId, reminderType, targetDate);

            if (existingReminder != null)
            {
                return; // Already scheduled
            }

            // Only schedule if reminder date is in the future
            if (reminderDate <= TimeZoneHelper.GetLocalTimeNow().Date)
            {
                return;
            }

            var emailReminderId = Guid.NewGuid();

            // Schedule the Hangfire job
            var jobId = _backgroundJobClient.Schedule<IEmailReminderBackgroundService>(
                service => service.SendScheduledEmailAsync(emailReminderId),
                reminderDate);

            await _bus.SendCommandAsync(new AddReminderCommand(
                emailReminderId,
                entityId,
                email,
                name,
                recipientType,
                reminderType,
                subject,
                message,
                targetDate,
                reminderDate,
                true,
                jobId
            ));

            _logger.LogInformation("Email reminder scheduled for {Email} on {ReminderDate} for {ReminderType}",
                email, reminderDate, reminderType);
        }

        public async Task SendScheduledEmailAsync(Guid emailReminderId)
        {
            try
            {
                var emailReminder = await _emailReminderRepository.GetByIdAsync(emailReminderId);

                if (emailReminder == null)
                {
                    _logger.LogWarning("Email reminder {EmailReminderId} not found", emailReminderId);
                    return;
                }

                if (emailReminder.IsSent)
                {
                    _logger.LogInformation("Email reminder {EmailReminderId} already sent", emailReminderId);
                    return;
                }

                // Publish message to MassTransit for email sending
                await _publishEndpoint.Publish<SendEmailReminderEvent>(new SendEmailReminderEvent
                (
                    emailReminder.Id,
                    emailReminder.RecipientEmail,
                    emailReminder.RecipientName,
                    (int)emailReminder.RecipientType,
                    (int)emailReminder.ReminderType,
                    emailReminder.Subject,
                    emailReminder.Message,
                    emailReminder.TargetDate
                ));

                // Mark as sent
                await _bus.SendCommandAsync(new UpdateReminderStatusCommand(
                    emailReminder.Id,
                    true
                ));

                _logger.LogInformation("Email reminder sent for {RecipientEmail}", emailReminder.RecipientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email reminder {EmailReminderId}", emailReminderId);
                throw;
            }
        }
    }
}
