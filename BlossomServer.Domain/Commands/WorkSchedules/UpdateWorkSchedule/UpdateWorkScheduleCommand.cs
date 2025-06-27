using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.WorkSchedules.UpdateWorkSchedule
{
    public sealed class UpdateWorkScheduleCommand : CommandBase, IRequest
    {
        private static readonly UpdateWorkScheduleCommandValidation s_validation = new();

        public Guid WorkScheduleId { get; }
        public DateOnly WorkDate { get; }
        public TimeOnly StartTime { get; }
        public TimeOnly EndTime { get; }
        public bool IsDayOff { get; }

        public UpdateWorkScheduleCommand(
            Guid workScheduleId,
            DateOnly workDate,
            TimeOnly startTime,
            TimeOnly endTime,
            bool isDayOff
        ) : base(Guid.NewGuid())
        {
            WorkScheduleId = workScheduleId;
            WorkDate = workDate;
            StartTime = startTime;
            EndTime = endTime;
            IsDayOff = isDayOff;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
