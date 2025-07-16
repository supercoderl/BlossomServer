using BlossomServer.Domain.Enums;

namespace BlossomServer.Application.ViewModels.Messages
{
    public sealed record CreateMessageViewModel
    (
        Guid RecipientId,
        Guid ConversationId,
        string MessageText,
        MessageType MessageType
    );
}
