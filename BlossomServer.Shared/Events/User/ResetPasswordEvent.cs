using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.User
{
    public sealed class ResetPasswordEvent : DomainEvent
    {
        public ResetPasswordEvent(Guid userId) : base(userId)
        {
            
        }
    }
}
