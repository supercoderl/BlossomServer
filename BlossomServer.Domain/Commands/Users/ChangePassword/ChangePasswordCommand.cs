using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ChangePassword
{
    public sealed class ChangePasswordCommand : CommandBase, IRequest
    {
        private static readonly ChangePasswordCommandValidation s_validation = new();

        public string OldPassword { get; }
        public string NewPassword { get; }

        public ChangePasswordCommand(
            string oldPassword,
            string newPassword
        ) : base(Guid.NewGuid())
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
