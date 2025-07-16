using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.DeleteServiceImage
{
    public sealed class DeleteServiceImageCommandValidation : AbstractValidator<DeleteServiceImageCommand>
    {
        public DeleteServiceImageCommandValidation()
        {
            RuleForId();
        }

        public void RuleForId()
        {
            RuleFor(cmd => cmd.ServiceImageId).NotEmpty().WithErrorCode(DomainErrorCodes.ServiceImage.EmptyId).WithMessage("Id may not be empty.");
        }
    }
}
