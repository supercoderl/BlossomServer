using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class BookingRepository : BaseRepository<Booking, Guid>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<(DateTime start, TimeSpan duration)>> GetScheduleTimes(Guid technicianId, DateTime selectedDate)
        {
            var bookings = await DbSet
                .Where(b => b.TechnicianId == technicianId)
                .Where(b => b.ScheduleTime.Date == selectedDate.Date)
                .Where(b => b.Status == Domain.Enums.BookingStatus.Pending || b.Status == Domain.Enums.BookingStatus.Confirmed)
                .ToListAsync();

            var result = new List<(DateTime start, TimeSpan duration)>();

            foreach (var booking in bookings)
            {
                int duration = await _context.BookingDetails
                    .Where(d => d.BookingId == booking.Id)
                    .Include(d => d.Service)
                    .Join(_context.ServiceOptions,
                        detail => detail.ServiceOptionId,
                        option => option.Id,
                        (detail, option) => option.Service != null ? option.Service.DurationMinutes : 0)
                    .FirstOrDefaultAsync() ?? 0;

                result.Add((booking.ScheduleTime, TimeSpan.FromMinutes(duration)));
            }

            return result;
        }
    }
}
