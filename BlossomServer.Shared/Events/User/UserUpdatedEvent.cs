using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.User
{
    public sealed class UserUpdatedEvent : DomainEvent
    {
        public UserUpdatedEvent(Guid userId) : base(userId)
        {

        }
    }
}
