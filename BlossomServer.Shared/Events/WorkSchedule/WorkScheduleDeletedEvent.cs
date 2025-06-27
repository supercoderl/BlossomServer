using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.WorkSchedule
{
    public sealed class WorkScheduleDeletedEvent : DomainEvent
    {
        public WorkScheduleDeletedEvent(Guid workScheduleId) : base(workScheduleId)
        {
            
        }
    }
}
