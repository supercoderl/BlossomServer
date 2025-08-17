using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ResetPassword
{
    public sealed class ResetPasswordCommand : CommandBase, IRequest
    {
        private static readonly ResetPasswordCommandValidation s_validation = new();

        public string Code { get; }
        public string NewPassword { get; }

        public ResetPasswordCommand(
            string code,
            string newPassword
        ) : base(Guid.NewGuid())
        {
            Code = code;
            NewPassword = newPassword;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
