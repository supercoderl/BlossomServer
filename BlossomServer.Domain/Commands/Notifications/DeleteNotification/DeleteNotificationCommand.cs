using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Notifications.DeleteNotification
{
    public sealed class DeleteNotificationCommand : CommandBase, IRequest
    {
        private static readonly DeleteNotificationCommandValidation s_validation = new();

        public Guid NotificationId { get; }

        public DeleteNotificationCommand(Guid notificationId) : base(Guid.NewGuid())
        {
            NotificationId = notificationId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
