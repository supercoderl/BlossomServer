using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.RefreshToken
{
    public sealed class RefreshTokenCommandValidation : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidation()
        {
            RuleForToken();   
        }

        public void RuleForToken()
        {
            RuleFor(cmd => cmd.RefreshToken).NotEmpty().WithErrorCode(DomainErrorCodes.RefreshToken.EmptyToken).WithMessage("Token may not be empty");
        }
    }
}
