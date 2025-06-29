using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.CreateServiceOption
{
    public sealed class CreateServiceOptionCommandValidation : AbstractValidator<CreateServiceOptionCommand>
    {
        public CreateServiceOptionCommandValidation()
        {
            RuleForVariantName();
        }

        public void RuleForVariantName()
        {
            RuleFor(cmd => cmd.VariantName).NotEmpty().WithErrorCode(DomainErrorCodes.ServiceOption.EmptyVariantName).WithMessage("Variant name may not be empty.");
        }
    }
}
