using BlossomServer.Domain.Errors;
using FluentValidation;

namespace BlossomServer.Domain.Commands.ContactResponses.CreateContactResponse
{
    public sealed class CreateContactResponseCommandValidation : AbstractValidator<CreateContactResponseCommand>
    {
        public CreateContactResponseCommandValidation()
        {
            RuleForResponseTest();
        }

        public void RuleForResponseTest()
        {
            RuleFor(cmd => cmd.ResponseText).NotEmpty().WithErrorCode(DomainErrorCodes.ContactResponse.EmptyResponseText).WithMessage("Response text may not be empty.");
        }
    }
}
