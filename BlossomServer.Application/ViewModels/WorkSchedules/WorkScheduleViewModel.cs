using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.WorkSchedules
{
    public sealed class WorkScheduleViewModel
    {
        public Guid Id { get; set; }
        public Guid TechnicianId { get; set; }
        public DateOnly WorkDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsDayOff { get; set; }

        public static WorkScheduleViewModel FromWorkSchedule(WorkSchedule workSchedule)
        {
            return new WorkScheduleViewModel
            {
                Id = workSchedule.Id,
                TechnicianId = workSchedule.TechnicianId,
                WorkDate = workSchedule.WorkDate,
                StartTime = workSchedule.StartTime,
                EndTime = workSchedule.EndTime,
                IsDayOff = workSchedule.IsDayOff
            };
        }
    }
}
