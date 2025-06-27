using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.UpdateServiceImage
{
    public sealed class UpdateServiceImageCommand : CommandBase, IRequest
    {
        private static readonly UpdateServiceImageCommandValidation s_validation = new();

        public Guid ServiceImageId { get; }
        public string ImageName { get; }
        public string? Description { get; }

        public UpdateServiceImageCommand(
            Guid serviceImageId,
            string imageName,
            string? description
        ) : base(Guid.NewGuid())
        {
            ServiceImageId = serviceImageId;
            ImageName = imageName;
            Description = description;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
