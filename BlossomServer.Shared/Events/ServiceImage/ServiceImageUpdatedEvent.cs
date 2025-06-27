using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ServiceImage
{
    public sealed class ServiceImageUpdatedEvent : DomainEvent
    {
        public ServiceImageUpdatedEvent(Guid serviceImageId) : base(serviceImageId)
        {
            
        }
    }
}
