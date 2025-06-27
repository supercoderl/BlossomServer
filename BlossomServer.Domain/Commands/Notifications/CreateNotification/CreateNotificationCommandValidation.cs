using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Notifications.CreateNotification
{
    public sealed class CreateNotificationCommandValidation : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidation()
        {
            
        }
    }
}
