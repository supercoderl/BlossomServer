using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Notification
{
    public sealed class NotificationCreatedEvent : DomainEvent
    {
        public NotificationCreatedEvent(Guid notificationId) : base(notificationId)
        {
            
        }
    }
}
