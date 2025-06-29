using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.UpdateServiceOption
{
    public sealed class UpdateServiceOptionCommandValidation : AbstractValidator<UpdateServiceOptionCommand>
    {
        public UpdateServiceOptionCommandValidation()
        {
            RuleForId();
            RuleForVariantName();
        }

        public void RuleForId()
        {
            RuleFor(cmd => cmd.ServiceOptionId).NotEmpty().WithErrorCode(DomainErrorCodes.ServiceOption.EmptyId).WithMessage("Id may not be empty.");
        }

        public void RuleForVariantName()
        {
            RuleFor(cmd => cmd.VariantName).NotEmpty().WithErrorCode(DomainErrorCodes.ServiceOption.EmptyVariantName).WithMessage("Variant name may not be empty.");
        }
    }
}
