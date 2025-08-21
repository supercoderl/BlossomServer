using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Notifications.CreateNotification
{
    public sealed class CreateNotificationCommand : CommandBase, IRequest
    {
        private static readonly CreateNotificationCommandValidation s_validation = new();

        public Guid NotificationId { get; }
        public Guid UserId { get; }
        public string Title { get; }
        public string Message { get; }
        public NotificationType NotificationType { get; }
        public int Priority { get; }
        public DateTime? ExpiresAt { get; }
        public string? ActionUrl { get; }
        public Guid? RelatedEntityId { get; }

        public CreateNotificationCommand(
            Guid notificationId,
            Guid userId,
            string title,
            string message,
            NotificationType notificationType,
            int priority,
            DateTime? expiresAt,
            string? actionUrl,
            Guid? relatedEntityId
        ) : base(Guid.NewGuid())
        {
            NotificationId = notificationId;
            UserId = userId;
            Title = title;
            Message = message;
            NotificationType = notificationType;
            Priority = priority;
            ExpiresAt = expiresAt;
            ActionUrl = actionUrl;
            RelatedEntityId = relatedEntityId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
