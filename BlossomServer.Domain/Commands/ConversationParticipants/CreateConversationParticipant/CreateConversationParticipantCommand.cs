using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ConversationParticipants.CreateConversationParticipant
{
    public sealed class CreateConversationParticipantCommand : CommandBase, IRequest
    {
        private static readonly CreateConversationParticipantCommandValidation s_validation = new();

        public Guid ConversationParticipantId { get; }
        public Guid ConversationId { get; }
        public Guid UserId { get; }

        public CreateConversationParticipantCommand(
            Guid conversationParticipantId,
            Guid conversationId,
            Guid userId
        ) : base(Guid.NewGuid())
        {
            ConversationParticipantId = conversationParticipantId;
            ConversationId = conversationId;
            UserId = userId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
