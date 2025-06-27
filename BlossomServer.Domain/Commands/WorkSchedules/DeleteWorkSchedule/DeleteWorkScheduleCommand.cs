using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.WorkSchedules.DeleteWorkSchedule
{
    public sealed class DeleteWorkScheduleCommand : CommandBase, IRequest
    {
        private static readonly DeleteWorkScheduleCommandValidation s_validation = new();

        public Guid WorkScheduleId { get; }

        public DeleteWorkScheduleCommand(Guid workScheduleId) : base(Guid.NewGuid())
        {
            WorkScheduleId = workScheduleId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
