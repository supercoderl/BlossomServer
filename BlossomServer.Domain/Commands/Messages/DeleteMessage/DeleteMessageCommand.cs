using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Messages.DeleteMessage
{
    public sealed class DeleteMessageCommand : CommandBase, IRequest
    {
        private static readonly DeleteMessageCommandValidation s_validation = new();

        public Guid? MessageId { get; }
        public Guid? ConversationId { get; }

        public DeleteMessageCommand(
            Guid? messageId,
            Guid? conversationId
        ) : base(Guid.NewGuid())
        {
            MessageId = messageId;
            ConversationId = conversationId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
