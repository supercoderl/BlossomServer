using FluentValidation;

namespace BlossomServer.Domain.Commands.Users.GenerateGuestToken
{
    public sealed class GenerateGuestTokenCommandValidation : AbstractValidator<GenerateGuestTokenCommand>
    {
        public GenerateGuestTokenCommandValidation()
        {

        }
    }
}
