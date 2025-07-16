using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class ConversationParticipant : Entity<Guid>
    {
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("ConversationId")]
        [InverseProperty("ConversationParticipants")]
        public virtual Conversation? Conversation { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ConversationParticipants")]
        public virtual User? User { get; set; }

        public ConversationParticipant(
            Guid id,
            Guid conversationId,
            Guid userId
        ) : base(id)
        {
            ConversationId = conversationId;
            UserId = userId;
        }

        public void SetConversationId( Guid conversationId ) { ConversationId = conversationId; }
        public void SetUserId( Guid userId ) { UserId = userId; }
    }
}
