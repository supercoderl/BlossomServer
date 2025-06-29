using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ServiceOption
{
    public sealed class ServiceOptionDeletedEvent : DomainEvent
    {
        public ServiceOptionDeletedEvent(Guid serviceOptionId) : base(serviceOptionId)
        {
            
        }
    }
}
