using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ContactResponses.CreateContactResponse
{
    public sealed class CreateContactResponseCommand : CommandBase, IRequest
    {
        private static readonly CreateContactResponseCommandValidation s_validation = new();

        public Guid ContactResponseId { get; }
        public Guid ContactId { get; }
        public string ResponseText { get; }
        public Guid ResponderId { get; }

        public CreateContactResponseCommand(
            Guid contactResponseId,
            Guid contactId,
            string responseText,
            Guid responderId
        ) : base(Guid.NewGuid())
        {
            ContactResponseId = contactResponseId;
            ContactId = contactId;
            ResponseText = responseText;
            ResponderId = responderId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
