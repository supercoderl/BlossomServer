using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Message
{
    public sealed class MessageCreatedEvent : DomainEvent
    {
        public MessageCreatedEvent(Guid messageId) : base(messageId)
        {
            
        }
    }
}
