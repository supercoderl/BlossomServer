using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ConversationParticipant
{
    public sealed class ConversationParticipantCreatedEvent : DomainEvent
    {
        public ConversationParticipantCreatedEvent(Guid conversationParticipantId) : base(conversationParticipantId)
        {
            
        }
    }
}
