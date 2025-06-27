using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.Bookings.GetById
{
    public sealed class GetBookingByIdQueryHandler :
        IRequestHandler<GetBookingByIdQuery, BookingViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly IBookingRepository _bookingRepository;

        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMediatorHandler bus)
        {
            _bookingRepository = bookingRepository;
            _bus = bus;
        }

        public async Task<BookingViewModel?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetBookingByIdQuery),
                        $"Booking with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return BookingViewModel.FromBooking(booking);
        }
    }
}
