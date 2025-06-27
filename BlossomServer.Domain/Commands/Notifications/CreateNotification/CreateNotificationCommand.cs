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

        public CreateNotificationCommand(
            Guid notificationId,
            Guid userId,
            string title,
            string message
        ) : base(Guid.NewGuid())
        {
            NotificationId = notificationId;
            UserId = userId;
            Title = title;
            Message = message;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
