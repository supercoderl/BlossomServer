using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Contacts.CreateContact
{
    public sealed class CreateContactCommandValidation : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidation()
        {
            RuleForName();
            RuleForEmail();
            RuleForMessage();
        }

        public void RuleForName()
        {
            RuleFor(cmd => cmd.Name).NotEmpty().WithErrorCode(DomainErrorCodes.Contact.EmptyName).WithMessage("Name may not be empty.");
        }

        public void RuleForEmail()
        {
            RuleFor(cmd => cmd.Email).NotEmpty().WithErrorCode(DomainErrorCodes.Contact.EmptyEmail).WithMessage("Email may not be empty.");
        }

        public void RuleForMessage()
        {
            RuleFor(cmd => cmd.Message).NotEmpty().WithErrorCode(DomainErrorCodes.Contact.EmptyMessage).WithMessage("Message may not be empty.");
        }
    }
}
