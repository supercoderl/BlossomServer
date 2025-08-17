using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.EmailReminders.UpdateStatus
{
    public sealed class UpdateReminderStatusCommandValidation : AbstractValidator<UpdateReminderStatusCommand>  
    {
        public UpdateReminderStatusCommandValidation()
        {
            
        }
    }
}
