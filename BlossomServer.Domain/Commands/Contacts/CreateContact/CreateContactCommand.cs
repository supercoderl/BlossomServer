using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Contacts.CreateContact
{
    public sealed class CreateContactCommand : CommandBase, IRequest
    {
        private static readonly CreateContactCommandValidation s_validation = new();

        public Guid ContactId { get; }
        public string Name { get; }
        public string Email { get; }
        public string Message { get; }

        public CreateContactCommand(
            Guid contactId,
            string name,
            string email,
            string message
        ) : base(Guid.NewGuid())
        {
            ContactId = contactId;
            Name = name;
            Email = email;
            Message = message;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
