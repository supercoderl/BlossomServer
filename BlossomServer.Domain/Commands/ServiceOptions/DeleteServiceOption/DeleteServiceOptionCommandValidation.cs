using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.DeleteServiceOption
{
    public sealed class DeleteServiceOptionCommandValidation : AbstractValidator<DeleteServiceOptionCommand>
    {
        public DeleteServiceOptionCommandValidation()
        {
            RuleForId();
        }

        public void RuleForId()
        {
            RuleFor(cmd => cmd.ServiceOptionId).NotEmpty().WithErrorCode(DomainErrorCodes.ServiceOption.EmptyId).WithMessage("Id may not be empty.");
        }
    }
}
