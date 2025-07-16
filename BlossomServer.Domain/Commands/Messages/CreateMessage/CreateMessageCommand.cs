using BlossomServer.Domain.Enums;
using MediatR;

namespace BlossomServer.Domain.Commands.Messages.CreateMessage
{
    public sealed class CreateMessageCommand : CommandBase, IRequest
    {
        private static readonly CreateMessageCommandValidation s_validation = new();

        public Guid MessageId { get; }
        public Guid SenderId { get; }
        public Guid RecipientId { get; }
        public Guid ConversationId { get; }
        public string MessageText { get; }
        public MessageType MessageType { get; }

        public CreateMessageCommand(
            Guid messageId,
            Guid senderId,
            Guid recipientId,
            Guid conversationId,
            string messageText,
            MessageType messageType
        ) : base(Guid.NewGuid())
        {
            MessageId = messageId;
            SenderId = senderId;
            RecipientId = recipientId;
            ConversationId = conversationId;
            MessageText = messageText;
            MessageType = messageType;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
