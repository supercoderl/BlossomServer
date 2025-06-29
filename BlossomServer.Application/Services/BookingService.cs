using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Bookings.GetAll;
using BlossomServer.Application.Queries.Bookings.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Bookings.CreateBooking;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMediatorHandler _bus;

        public BookingService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> CreateBookingAsync(CreateBookingViewModel booking)
        {
            var bookingId = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateBookingCommand(
                bookingId,
                booking.CustomerId,
                booking.TechnicianId,
                booking.ScheduleTime,
                booking.ServiceId,
                booking.ServiceOptionId,
                booking.Quantity,
                booking.Price,
                booking.Note,
                booking.GuestName,
                booking.GuestPhone,
                booking.GuestEmail
            ));

            return bookingId;
        }

        public async Task DeleteBookingAsync(Guid bookingId)
        {
            await Task.CompletedTask;
        }

        public async Task<PagedResult<BookingViewModel>> GetAllBookingsAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllBookingsQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<BookingViewModel?> GetBookingByBookingIdAsync(Guid bookingId)
        {
            return await _bus.QueryAsync(new GetBookingByIdQuery(bookingId));
        }
    }
}
