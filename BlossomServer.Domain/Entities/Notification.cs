using BlossomServer.Domain.Enums;
using BlossomServer.SharedKernel.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlossomServer.Domain.Entities
{
    public class Notification : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public bool IsRead { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public int Priority { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public string? ActionUrl { get; private set; }
        public Guid? RelatedEntityId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("UserId")]
        [InverseProperty("Notifications")]
        public virtual User? User { get; set; }

        public Notification(
            Guid id,
            Guid userId,
            string title,
            string message,
            NotificationType notificationType,
            int priority,
            DateTime? expiresAt,
            string? actionUrl,
            Guid? relatedEntityId
        ) : base(id)
        {
            UserId = userId;
            Title = title;
            Message = message;
            IsRead = false;
            NotificationType = notificationType;
            Priority = priority;
            ExpiresAt = expiresAt;
            ActionUrl = actionUrl;
            RelatedEntityId = relatedEntityId;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetUserId(Guid userId) { UserId = userId; }
        public void SetTitle(string title) { Title = title; }
        public void SetMessage(string message) { Message = message; }
        public void SetIsRead(bool isRead) { IsRead = isRead; }
        public void SetNotificationType(NotificationType notificationType) { NotificationType = notificationType; }
        public void SetPriority(int priority) { Priority = priority; }
        public void SetExpiresAt(DateTime? expiresAt) { ExpiresAt = expiresAt; }
        public void SetActionUrl(string? actionUrl) { ActionUrl = actionUrl; }
        public void SetRelatedEntityId(Guid? relatedEntityId) { RelatedEntityId = relatedEntityId; }
        public void SetCreatedAt(DateTime createdAt) { CreatedAt = createdAt; }
    }
}
