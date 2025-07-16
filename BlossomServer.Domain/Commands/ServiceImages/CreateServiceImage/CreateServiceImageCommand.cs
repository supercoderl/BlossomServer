using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.CreateServiceImage
{
    public sealed class CreateServiceImageCommand : CommandBase, IRequest
    {
        private static readonly CreateServiceImageCommandValidation s_validation = new();

        public Guid ServiceImageId { get; }
        public List<IFormFile> ImageFile { get; }
        public Guid ServiceId { get; }
        public string? Description { get; }

        public CreateServiceImageCommand(
            Guid serviceImageId,
            List<IFormFile> imageFile,
            Guid serviceId,
            string? description
        ) : base(Guid.NewGuid())
        {
            ServiceImageId = serviceImageId;
            ImageFile = imageFile;
            ServiceId = serviceId;
            Description = description;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
