using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Extensions.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ChangePassword
{
    public sealed class ChangePasswordCommandValidation : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidation()
        {
            RuleForOldPassword();
            RuleForPassword();
        }

        public void RuleForOldPassword()
        {
            RuleFor(cmd => cmd.OldPassword)
                .Password();
        }

        public void RuleForPassword()
        {
            RuleFor(cmd => cmd.NewPassword)
                .Password();
        }
    }
}
