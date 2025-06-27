using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Technicians.UpdateTechnician
{
    public sealed class UpdateTechnicianCommand : CommandBase, IRequest
    {
        private static readonly UpdateTechnicianCommandValidation s_validation = new();

        public Guid TechnicianId { get; }
        public string Bio { get; }
        public double Rating { get; }
        public int YearsOfExperience { get; }

        public UpdateTechnicianCommand(
            Guid technicianId,
            string bio,
            double rating,
            int yearsOfExperience
        ) : base(Guid.NewGuid())
        {
            TechnicianId = technicianId;
            Bio = bio;
            Rating = rating;
            YearsOfExperience = yearsOfExperience;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
