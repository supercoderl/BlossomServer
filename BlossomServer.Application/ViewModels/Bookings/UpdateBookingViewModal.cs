using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Bookings
{
    public sealed record UpdateBookingStatusViewModel
    (
        Guid Id,
        BookingStatus Status
    );
}
