using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Notifications.UpdateStatusNotification
{
    public sealed class UpdateStatusNotificationCommand : CommandBase, IRequest
    {
        private static readonly UpdateStatusNotificationCommandValidation s_validation = new();

        public Guid NotificationId { get; }

        public UpdateStatusNotificationCommand(Guid notificationId) : base(Guid.NewGuid())
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
