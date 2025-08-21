using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IBookingRepository : IRepository<Booking, Guid>
    {
        Task<IEnumerable<(DateTime start, TimeSpan duration)>> GetScheduleTimes(Guid technicianId, DateTime selectedDate);
        Task<IEnumerable<Booking>> GetAllBookingsBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<Booking>> GetAllBookingsIncommingBySQL(
           int page,
           int pageSize,
           CancellationToken cancellationToken = default
        );

        Task<object> GetBookingCountSQL(string dateStart, string dateEnd, CancellationToken cancellationToken);

        Task<int> GetCustomerCountSQL(string dateStart, string dateEnd, CancellationToken cancellationToken);

        Task<object> CalculateRevenueSQL(string dateStart, string dateEnd, CancellationToken cancellationToken);

        Task<IEnumerable<object>> GetBookingStatusBreakdownSQL(string dateStart, string dateEnd, CancellationToken cancellationToken);

        Task<object> GetCustomerRetentionRateSQL(
            string currentDateStart, 
            string currentDateEnd,
            string previousDateStart,
            string previousDateEnd,
            CancellationToken cancellationToken
        );

        Task<decimal> GetConversionRateSQL(string dateStart, string dateEnd, CancellationToken cancellationToken);

        Task<IEnumerable<object>> GetScheduleByDateSQL(string date, CancellationToken cancellationToken);
    }
}
