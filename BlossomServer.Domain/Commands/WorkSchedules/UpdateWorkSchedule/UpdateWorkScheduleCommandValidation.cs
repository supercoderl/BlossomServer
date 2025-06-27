using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.WorkSchedules.UpdateWorkSchedule
{
    public sealed class UpdateWorkScheduleCommandValidation : AbstractValidator<UpdateWorkScheduleCommand>
    {
        public UpdateWorkScheduleCommandValidation()
        {

        }
    }
}
