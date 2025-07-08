using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Bookings
{
    public sealed class ScheduleSlot
    {
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
    }
}