using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.WorkSchedule
{
    public sealed class WorkScheduleUpdatedEvent : DomainEvent
    {
        public WorkScheduleUpdatedEvent(Guid workScheduleId) : base(workScheduleId)
        {
            
        }
    }
}
