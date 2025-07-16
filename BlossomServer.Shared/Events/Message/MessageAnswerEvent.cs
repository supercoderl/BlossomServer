using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Message
{
    public sealed class MessageAnswerEvent : DomainEvent
    {
        public string Answer { get; set; }
        public Guid ConversationId { get; set; }

        public MessageAnswerEvent(
            Guid recipientId,
            string answer,
            Guid conversationId
        ) : base(recipientId)
        {
            Answer = answer;
            ConversationId = conversationId;
        }
    }
}
