using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ServiceOption
{
    public sealed class ServiceOptionCreatedEvent : DomainEvent
    {
        public ServiceOptionCreatedEvent(Guid serviceOptionId) : base(serviceOptionId)
        {
            
        }
    }
}
