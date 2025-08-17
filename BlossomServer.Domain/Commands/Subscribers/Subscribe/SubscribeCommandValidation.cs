using BlossomServer.Domain.Errors;
using FluentValidation;

namespace BlossomServer.Domain.Commands.Subscribers.Subscribe
{
    public sealed class SubscribeCommandValidation : AbstractValidator<SubscribeCommand>
    {
        public SubscribeCommandValidation()
        {
            RuleForEmail();
        }

        public void RuleForEmail()
        {
            RuleFor(cmd => cmd.Email)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.Subcriber.EmptyEmail)
                .WithMessage("Email may not be empty.")
                .EmailAddress()
                .WithErrorCode(DomainErrorCodes.Subcriber.InvalidEmail)
                .WithMessage("Invalid email format.");
        }
    }
}
