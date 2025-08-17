using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ForgotPassword
{
    public sealed class ForgotPasswordCommandValidation : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidation()
        {
            RuleForIdentifier();
        }

        public void RuleForIdentifier()
        {
            RuleFor(cmd => cmd.Identifier).NotEmpty().WithMessage("Identifier cannot be empty.");
        }
    }
}
