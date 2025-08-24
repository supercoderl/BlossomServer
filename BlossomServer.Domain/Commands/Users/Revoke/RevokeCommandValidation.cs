using FluentValidation;

namespace BlossomServer.Domain.Commands.Users.Revoke
{
    public sealed class RevokeCommandValidation : AbstractValidator<RevokeCommand>
    {
        public RevokeCommandValidation()
        {
            RuleForToken();
        }

        public void RuleForToken()
        {
            RuleFor(cmd => cmd.RefreshToken).NotEmpty().WithMessage("Refresh token may not be empty.");
        }
    }
}
