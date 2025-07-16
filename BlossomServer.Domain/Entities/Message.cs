using BlossomServer.Domain.Enums;
using BlossomServer.SharedKernel.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlossomServer.Domain.Entities
{
    public class Message : Entity<Guid>
    {
        public Guid SenderId { get; private set; }
        public Guid? RecipientId { get; private set; }
        public Guid ConversationId { get; private set; }
        public string MessageText { get; private set; }
        public MessageType MessageType { get; private set; }
        public bool IsRead { get; private set; }
        public int UnreadCount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }

        [ForeignKey("SenderId")]
        [InverseProperty("SenderMessages")]
        public virtual User? Sender { get; set; }

        [ForeignKey("RecipientId")]
        [InverseProperty("RecipientMessages")]
        public virtual User? Recipient { get; set; }

        [ForeignKey("ConversationId")]
        [InverseProperty("Messages")]
        public virtual Conversation? Conversation { get; set; }

        public Message(
            Guid id,
            Guid senderId,
            Guid? recipientId,
            Guid conversationId,
            string messageText,
            MessageType messageType
        ) : base(id)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            ConversationId = conversationId;
            MessageText = messageText;
            MessageType = messageType;
            IsRead = false;
            UnreadCount = 0;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetSenderId(Guid senderId) { SenderId = senderId; }
        public void SetRecipientId(Guid? recipientId) { RecipientId = recipientId; }
        public void SetConversationId(Guid conversationId) { ConversationId = conversationId; }
        public void SetMessageType(MessageType messageType) { MessageType = messageType; }
        public void SetMessageText(string messageText) { MessageText = messageText; }
        public void SetIsRead(bool isRead) { IsRead = isRead; }
        public void SetUnreadCount(int unreadCount) { UnreadCount = unreadCount; }
        public void SetLastUpdatedAt() { LastUpdatedAt = TimeZoneHelper.GetLocalTimeNow(); }
    }
}
