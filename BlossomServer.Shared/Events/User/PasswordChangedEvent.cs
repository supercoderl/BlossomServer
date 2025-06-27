using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.User
{
    public sealed class PasswordChangedEvent : DomainEvent
    {
        public PasswordChangedEvent(Guid userId) : base(userId)
        {
        }
    }
}
