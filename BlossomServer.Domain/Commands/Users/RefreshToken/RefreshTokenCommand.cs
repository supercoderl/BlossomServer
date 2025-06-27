using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.RefreshToken
{
    public sealed class RefreshTokenCommand : CommandBase, IRequest<object>
    {
        private static readonly RefreshTokenCommandValidation s_validation = new();

        public string RefreshToken { get; }

        public RefreshTokenCommand(string refreshToken): base(Guid.NewGuid())
        {
            RefreshToken = refreshToken;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
