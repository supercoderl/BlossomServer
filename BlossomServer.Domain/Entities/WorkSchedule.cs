using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class WorkSchedule : Entity<Guid>
    {
        public Guid TechnicianId { get; private set; }
        public DateOnly WorkDate { get; private set; }  
        public TimeOnly StartTime { get; private set; }
        public TimeOnly EndTime { get; private set; }
        public bool IsDayOff { get; private set; }

        [ForeignKey("TechnicianId")]
        [InverseProperty("WorkSchedules")]
        public virtual Technician? Technician { get; set; }

        public WorkSchedule(
            Guid id,
            Guid technicianId,
            DateOnly workDate,
            TimeOnly startTime,
            TimeOnly endTime,
            bool isDayOff
        ) : base(id)
        {
            TechnicianId = technicianId;
            WorkDate = workDate;
            StartTime = startTime;
            EndTime = endTime;
            IsDayOff = isDayOff;
        }

        public void SetTechnicianId(Guid technicianId) { TechnicianId = technicianId; }
        public void SetWorkDate(DateOnly workDate) { WorkDate = workDate; }
        public void SetStartTime(TimeOnly startTime) { StartTime = startTime; }
        public void SetEndTime(TimeOnly endTime) { EndTime = endTime; }
        public void SetIsDayOff(bool isDayOff) { IsDayOff = isDayOff; }
    }
}
