using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ForgotPassword
{
    public sealed class ForgotPasswordCommand : CommandBase, IRequest
    {
        private static readonly ForgotPasswordCommandValidation s_validation = new();

        public Guid PasswordResetTokenId { get; }
        public string Identifier { get; }

        public ForgotPasswordCommand(Guid passwordResetTokenId, string identifier) : base(Guid.NewGuid())
        {
            PasswordResetTokenId = passwordResetTokenId;
            Identifier = identifier;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
