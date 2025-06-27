using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Technician
{
    public sealed class TechnicianDeletedEvent : DomainEvent
    {
        public TechnicianDeletedEvent(Guid technicianId) : base(technicianId)
        {
            
        }
    }
}
