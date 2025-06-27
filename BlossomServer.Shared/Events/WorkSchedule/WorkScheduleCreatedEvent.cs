using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.WorkSchedule
{
    public sealed class WorkScheduleCreatedEvent : DomainEvent
    {
        public WorkScheduleCreatedEvent(Guid workScheduleId) : base(workScheduleId)
        {
            
        }
    }
}
