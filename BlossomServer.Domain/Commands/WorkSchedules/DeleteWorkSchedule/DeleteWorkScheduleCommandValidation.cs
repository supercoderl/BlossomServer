using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.WorkSchedules.DeleteWorkSchedule
{
    public sealed class DeleteWorkScheduleCommandValidation : AbstractValidator<DeleteWorkScheduleCommand>
    {
        public DeleteWorkScheduleCommandValidation()
        {
            
        }
    }
}
