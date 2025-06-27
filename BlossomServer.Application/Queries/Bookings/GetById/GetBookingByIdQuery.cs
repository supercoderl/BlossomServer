using BlossomServer.Application.ViewModels.Bookings;
using MediatR;

namespace BlossomServer.Application.Queries.Bookings.GetById
{
    public sealed record GetBookingByIdQuery(Guid Id) : IRequest<BookingViewModel?>;
}
