using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.RefreshToken.CreateRefreshToken
{
    public sealed class CreateRefreshTokenCommandValidation : AbstractValidator<CreateRefreshTokenCommand>
    {
        public CreateRefreshTokenCommandValidation()
        {
            RuleForUserId();
            RuleForRefreshToken();
        }

        public void RuleForUserId()
        {
            RuleFor(cmd => cmd.UserId).NotEmpty().WithErrorCode(DomainErrorCodes.RefreshToken.EmptyUserId).WithMessage("User id may not be empty.");
        }

        public void RuleForRefreshToken()
        {
            RuleFor(cmd => cmd.Token).NotEmpty().WithErrorCode(DomainErrorCodes.RefreshToken.EmptyToken).WithMessage("Token may not be emtpy.");
        }
    }
}
