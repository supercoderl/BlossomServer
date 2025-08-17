using BlossomServer.Domain.Enums;
using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class EmailReminder : Entity<Guid>
    {
        public Guid EntityId { get; private set; }
        public string RecipientEmail { get; private set; }
        public string RecipientName { get; private set; }
        public RecipientType RecipientType { get; private set; }
        public EmailReminderType ReminderType { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }
        public DateTime TargetDate { get; private set; }
        public DateTime ReminderDate { get; private set; }
        public bool IsScheduled { get; private set; }
        public bool IsSent { get; private set; }
        public string HangfireJobId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public EmailReminder(
            Guid id,
            Guid entityId,
            string recipientEmail,
            string recipientName,
            RecipientType recipientType,
            EmailReminderType reminderType,
            string subject,
            string message,
            DateTime targetDate,
            DateTime reminderDate,
            bool isScheduled,
            string hangfireJobId
        ) : base(id)
        {
            EntityId = entityId;
            RecipientEmail = recipientEmail;
            RecipientName = recipientName;
            RecipientType = recipientType;
            ReminderType = reminderType;
            Subject = subject;
            Message = message;
            TargetDate = targetDate;
            ReminderDate = reminderDate;
            IsScheduled = isScheduled;
            IsSent = false;
            HangfireJobId = hangfireJobId;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetEntityId(Guid entityId) { EntityId = entityId; }
        public void SetRecipientEmail(string recipientEmail) { RecipientEmail = recipientEmail; }
        public void SetRecipientName(string recipientName) { RecipientName = recipientName; }
        public void SetRecipientType(RecipientType recipientType) { RecipientType = recipientType; }
        public void SetReminderType(EmailReminderType reminderType) { ReminderType= reminderType; }
        public void SetSubject(string subject) { Subject = subject; }
        public void SetMessage(string message) { Message = message; }
        public void SetTargetDate(DateTime targetDate) { TargetDate = targetDate; }
        public void SetReminderDate(DateTime reminderDate) { ReminderDate = reminderDate; }
        public void SetIsScheduled(bool isScheduled) { IsScheduled = isScheduled; }
        public void SetIsSent(bool isSent) { IsSent = isSent; }
        public void SetHangfireJobId(string hangfireJobId) { HangfireJobId= hangfireJobId; }
        public void SetCreatedAt(DateTime createdAt) { CreatedAt = createdAt; }
    }
}
