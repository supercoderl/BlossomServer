using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.EmailReminder
{
    public sealed class SendEmailReminderEvent : DomainEvent
    {
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public int RecipientType { get; }
        public int ReminderType { get; }
        public string Subject { get; }
        public string Message { get; }
        public DateTime TargetDate { get; }

        public SendEmailReminderEvent(
            Guid emailReminderId,
            string recipientEmail,
            string recipientName,
            int recipientType,
            int reminderType,
            string subject,
            string message,
            DateTime targetDate

        ) : base(emailReminderId)
        {
            RecipientEmail = recipientEmail;
            RecipientName = recipientName;
            RecipientType = recipientType;
            ReminderType = reminderType;
            Subject = subject;
            Message = message;
            TargetDate = targetDate;
        }
    }
}
