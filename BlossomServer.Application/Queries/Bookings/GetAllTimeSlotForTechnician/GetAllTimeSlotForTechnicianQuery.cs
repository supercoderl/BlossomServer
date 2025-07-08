using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Queries.Bookings.GetAllTimeSlotForTechnician
{
    public sealed record GetAllTimeSlotForTechnicianQuery(
        Guid technicianId,
        DateTime selectedDate 
    ) : IRequest<IEnumerable<(DateTime start, TimeSpan duration)>>;
}
