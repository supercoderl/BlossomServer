using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Notifications.DeleteNotification
{
    public sealed class DeleteNotificationCommandValidation : AbstractValidator<DeleteNotificationCommand>
    {
        public DeleteNotificationCommandValidation()
        {
            
        }
    }
}
