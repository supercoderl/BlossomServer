using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IBookingService
    {
        public Task<BookingViewModel?> GetBookingByBookingIdAsync(Guid bookingId);

        public Task<PagedResult<BookingViewModel>> GetAllBookingsAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);
        public Task<IEnumerable<ScheduleSlot>> GetAllTimeSlotForTechinicianAsync(
            Guid technicianId,
            DateTime selectedDate
        );

        public Task<Guid> CreateBookingAsync(CreateBookingViewModel booking);
        public Task UpdateBookingStatusAsync(UpdateBookingStatusViewModel booking);
        public Task DeleteBookingAsync(Guid bookingId);
        public Task<IEnumerable<object>> GetScheduleByDateAsync(string date);
    }
}
