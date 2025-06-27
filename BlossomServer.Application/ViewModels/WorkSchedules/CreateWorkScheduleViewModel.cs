using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.WorkSchedules
{
    public sealed record CreateWorkScheduleViewModel
    (
        Guid TechnicianId,
        DateOnly WorkDate,
        TimeOnly StartTime,
        TimeOnly EndTime,
        bool IsDayOff
    );
}
