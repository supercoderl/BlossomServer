using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.EmailReminders.AddReminder
{
    public sealed class AddReminderCommandValidation : AbstractValidator<AddReminderCommand>    
    {
        public AddReminderCommandValidation()
        {
            
        }
    }
}
