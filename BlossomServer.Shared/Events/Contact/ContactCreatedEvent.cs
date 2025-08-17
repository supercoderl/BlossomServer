using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Contact
{
    public sealed class ContactCreatedEvent : DomainEvent
    {
        public ContactCreatedEvent(Guid contactId) : base(contactId)
        {
            
        }
    }
}
