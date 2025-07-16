using BlossomServer.Domain.Enums;
using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Conversation : Entity<Guid>
    {
        public string Name { get; private set; }
        public ConversationType ConversationType { get; private set; }
        public Guid CreatedBy { get; private set; }
        public Guid? LastMessageId { get; private set; }
        public DateTime LastActivity {  get; private set; }
        public DateTime CreatedAt { get; private set; }

        [InverseProperty("Conversation")]
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        [InverseProperty("Conversation")]
        public virtual ICollection<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();

        public Conversation(
            Guid id,
            string name,
            ConversationType conversationType,
            Guid createdBy,
            Guid? lastMessageId
        ) : base( id )
        {
            Name = name;
            ConversationType = conversationType;
            CreatedBy = createdBy;
            LastMessageId = lastMessageId;
            LastActivity = TimeZoneHelper.GetLocalTimeNow();
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetName( string name ) { Name = name; }
        public void SetConversationType( ConversationType conversationType ) { ConversationType = conversationType; }
        public void SetLastMessageId( Guid? lastMessageId ) { LastMessageId = lastMessageId; }
        public void SetLastActivity() { LastActivity = TimeZoneHelper.GetLocalTimeNow(); }
    }
}
