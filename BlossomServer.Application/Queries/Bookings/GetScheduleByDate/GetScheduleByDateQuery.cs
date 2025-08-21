using MediatR;

namespace BlossomServer.Application.Queries.Bookings.GetScheduleByDate
{
    public sealed record GetScheduleByDateQuery(string date) : IRequest<IEnumerable<object>>;
}
