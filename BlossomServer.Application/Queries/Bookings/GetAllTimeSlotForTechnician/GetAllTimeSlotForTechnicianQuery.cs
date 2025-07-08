using BlossomServer.Application.ViewModels.Bookings;
using MediatR;

namespace BlossomServer.Application.Queries.Bookings.GetAllTimeSlotForTechnician
{
    public sealed record GetAllTimeSlotForTechnicianQuery(
        Guid technicianId,
        DateTime selectedDate
    ) : IRequest<IEnumerable<ScheduleSlot>>;
}
