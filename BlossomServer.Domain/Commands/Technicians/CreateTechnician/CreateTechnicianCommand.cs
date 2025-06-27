using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Technicians.CreateTechnician
{
    public sealed class CreateTechnicianCommand : CommandBase, IRequest
    {
        private static readonly CreateTechnicianCommandValidation s_validation = new();

        public Guid TechnicianId { get; }
        public Guid UserId { get; }
        public string Bio { get; }
        public double Rating { get; }
        public int YearsOfExperience { get; }

        public CreateTechnicianCommand(
            Guid technicianId,
            Guid userId,
            string bio,
            double rating,
            int yearsOfExperience
        ) : base(Guid.NewGuid())
        {
            TechnicianId = technicianId;
            UserId = userId;
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
