using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.EmailReminders.UpdateReminder
{
    public sealed class UpdateReminderCommand : CommandBase, IRequest
    {
        private static readonly UpdateReminderCommandValidation s_validation = new();

        public Guid ReminderId { get; }
        public Guid EntityId { get; }
        public string RecipientEmail { get; }
        public string RecipientName { get; }
        public RecipientType RecipientType { get; }
        public EmailReminderType ReminderType { get; }
        public string Subject { get; }
        public string Message { get; }
        public DateTime TargetDate { get; }
        public DateTime ReminderDate { get; }
        public bool IsScheduled { get; }
        public string HangfireJobId { get; }

        public UpdateReminderCommand(
            Guid reminderId,
            Guid entityId,
            string recipientEmail,
            string recipientName,
            RecipientType recipientType,
            EmailReminderType emailReminderType,
            string subject,
            string message,
            DateTime targetDate,
            DateTime reminderDate,
            bool isScheduled,
            string hangfireJobId
        ) : base(Guid.NewGuid())
        {
            ReminderId = reminderId;
            EntityId = entityId;
            RecipientEmail = recipientEmail;
            RecipientName = recipientName;
            RecipientType = recipientType;
            ReminderType = emailReminderType;
            Subject = subject;
            Message = message;
            TargetDate = targetDate;
            ReminderDate = reminderDate;
            IsScheduled = isScheduled;
            HangfireJobId = hangfireJobId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
