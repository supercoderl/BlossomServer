﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Technician
{
    public sealed class TechnicianUpdatedEvent : DomainEvent
    {
        public TechnicianUpdatedEvent(Guid technicianId) : base(technicianId)
        {
            
        }
    }
}
