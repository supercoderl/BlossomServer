using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ServiceImage
{
    public sealed class ServiceImageCreatedEvent : DomainEvent
    {
        public ServiceImageCreatedEvent(Guid serviceImageId) : base(serviceImageId)
        {
            
        }
    }
}
