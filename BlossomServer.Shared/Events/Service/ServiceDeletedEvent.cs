using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Service
{
    public sealed class ServiceDeletedEvent : DomainEvent
    {
        public ServiceDeletedEvent(Guid serviceId) : base(serviceId)
        {
            
        }
    }
}
