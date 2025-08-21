using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Notifications
{
    public sealed class NotificationViewModel
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
        public int Priority { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? ActionUrl { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public DateTime CreatedAt { get; set; }

        public static NotificationViewModel FromNotification(Notification notification)
        {
            return new NotificationViewModel
            {
                NotificationId = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = notification.IsRead,
                NotificationType = notification.NotificationType,
                Priority = notification.Priority,
                ExpiresAt = notification.ExpiresAt,
                ActionUrl = notification.ActionUrl,
                RelatedEntityId = notification.RelatedEntityId,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}
