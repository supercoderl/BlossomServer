using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.EmailReminders.UpdateReminder
{
    public sealed class UpdateReminderCommandValidation : AbstractValidator<UpdateReminderCommand>
    {
        public UpdateReminderCommandValidation()
        {
            
        }
    }
}
