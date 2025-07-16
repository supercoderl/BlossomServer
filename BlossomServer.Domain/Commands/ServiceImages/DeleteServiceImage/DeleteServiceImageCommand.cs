using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.DeleteServiceImage
{
    public sealed class DeleteServiceImageCommand : CommandBase, IRequest
    {
        private static readonly DeleteServiceImageCommandValidation s_validation = new();

        public Guid ServiceImageId { get; }

        public DeleteServiceImageCommand(
            Guid serviceImageId    
        ) : base(Guid.NewGuid())
        {
            ServiceImageId = serviceImageId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
