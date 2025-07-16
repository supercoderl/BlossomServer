using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ServiceImage
{
    public sealed class ServiceImageDeletedEvent : DomainEvent
    {
        public ServiceImageDeletedEvent(Guid serviceImageId) : base(serviceImageId)
        {
            
        }
    }
}
