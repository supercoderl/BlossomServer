using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.WorkSchedules.CreateWorkSchedule
{
    public sealed class CreateWorkScheduleCommand : CommandBase, IRequest
    {
        private static readonly CreateWorkScheduleCommandValidation s_validation = new();

        public Guid WorkScheduleId { get; }
        public Guid TechnicianId { get; }
        public DateOnly WorkDate { get; }
        public TimeOnly StartTime { get; }
        public TimeOnly EndTime { get; }
        public bool IsDayOff { get; }

        public CreateWorkScheduleCommand(
            Guid workScheduleId,
            Guid technicianId,
            DateOnly workDate,
            TimeOnly startTime,
            TimeOnly endTime,
            bool isDayOff
        ) : base(Guid.NewGuid())
        {
            WorkScheduleId = workScheduleId;
            TechnicianId = technicianId;
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
