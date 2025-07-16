using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Conversations.CreateConversation
{
    public sealed class CreateConversationCommand : CommandBase, IRequest
    {
        private static readonly CreateConversationCommandValidation s_validation = new();

        public Guid ConversationId { get; }
        public string Name { get; }
        public ConversationType ConversationType { get; }
        public Guid CreatedBy { get; }
        public Guid RecipientId { get; }

        public CreateConversationCommand(
            Guid conversationId,
            string name,
            ConversationType conversationType,
            Guid createdBy,
            Guid recipientId
        ) : base(Guid.NewGuid())
        {
            ConversationId = conversationId;    
            Name = name;
            ConversationType = conversationType;
            CreatedBy = createdBy;
            RecipientId = recipientId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
