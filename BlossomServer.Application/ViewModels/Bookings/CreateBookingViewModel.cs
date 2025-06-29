using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Bookings
{
    public sealed record CreateBookingViewModel
    (
        Guid? CustomerId,
        Guid? TechnicianId,
        DateTime ScheduleTime,
        Guid? ServiceId,
        Guid? ServiceOptionId,
        int Quantity,
        decimal Price,
        string? Note,
        string? GuestName,
        string? GuestPhone,
        string? GuestEmail
    );
}
