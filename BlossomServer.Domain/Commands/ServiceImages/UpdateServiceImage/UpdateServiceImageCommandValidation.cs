using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.UpdateServiceImage
{
    public sealed class UpdateServiceImageCommandValidation : AbstractValidator<UpdateServiceImageCommand>
    {
        public UpdateServiceImageCommandValidation()
        {
            
        }

        public void RuleForId()
        {
            RuleFor(cmd => cmd.ServiceImageId).NotEmpty().WithErrorCode(DomainErrorCodes.ServiceImage.EmptyId).WithMessage("Id may not be emtpy.");
        }

        public void RuleForName()
        {
            RuleFor(cmd => cmd.ImageName).NotEmpty().WithErrorCode(DomainErrorCodes.ServiceImage.EmptyName).WithMessage("Name may not be emtpy.");
        }
    }
}
