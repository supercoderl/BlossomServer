using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Technicians.DeleteTechnician
{
    public sealed class DeleteTechnicianCommand : CommandBase, IRequest
    {
        private static readonly DeleteTechnicianCommandValidation s_validation = new();

        public Guid TechnicianId { get; }

        public DeleteTechnicianCommand(Guid technicianId) : base(Guid.NewGuid())
        {
            TechnicianId = technicianId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
