using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ContactResponse
{
    public sealed class ContactResponseCreatedEvent : DomainEvent
    {
        public string Email { get; set; }
        public string ResponseText { get; set; }

        public ContactResponseCreatedEvent(Guid contactResponseId, string email, string responseText) : base(contactResponseId)
        {
            Email = email;
            ResponseText = responseText;
        }
    }
}
