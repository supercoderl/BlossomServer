using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ResetPassword
{
    public sealed class ResetPasswordCommandValidation : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidation()
        {
            RuleForCode();
            RuleForPassword();
        }

        public void RuleForCode()
        {
            RuleFor(cmd => cmd.Code).NotEmpty().WithMessage("Code may not be empty.");
        }

        public void RuleForPassword()
        {
            RuleFor(cmd => cmd.NewPassword).NotEmpty().WithMessage("Password may not be empty.");
        }
    }
}
