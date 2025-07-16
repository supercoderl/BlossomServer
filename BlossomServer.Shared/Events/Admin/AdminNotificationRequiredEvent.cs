using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Admin
{
    public sealed class AdminNotificationRequiredEvent : DomainEvent
    {
        public string Message { get; }

        public AdminNotificationRequiredEvent(
            string message    
        ) : base(Guid.NewGuid())
        {
            Message = message;
        }
    }
}
