using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Dashboards.Admin
{
    public sealed class GetBusinessAnalyticsQueryHandler :
                IRequestHandler<GetBusinessAnalyticsQuery, object>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IServiceRepository _serviceRepository;

        public GetBusinessAnalyticsQueryHandler(
            IBookingRepository bookingRepository,
            IServiceRepository serviceRepository
        )
        {
            _bookingRepository = bookingRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<object> Handle(
            GetBusinessAnalyticsQuery request,
            CancellationToken cancellationToken)
        {
            var currentRange = request.CurrentRange;
            var previousRange = request.PreviousRange;

            var revenueTask = _bookingRepository.CalculateRevenueSQL(currentRange.DateStart, currentRange.DateEnd, cancellationToken);
            var bookingsTask = _bookingRepository.GetBookingCountSQL(currentRange.DateStart, currentRange.DateEnd, cancellationToken);
            var customersTask = _bookingRepository.GetCustomerCountSQL(currentRange.DateStart, currentRange.DateEnd, cancellationToken);
            var avgValueTask = _serviceRepository.GetAverageServiceValue(currentRange.DateStart, currentRange.DateEnd, cancellationToken);
            var conversionTask = _bookingRepository.GetConversionRateSQL(currentRange.DateStart, currentRange.DateEnd, cancellationToken);
            var retentionTask = _bookingRepository.GetCustomerRetentionRateSQL(currentRange.DateStart, currentRange.DateEnd, previousRange.DateStart, previousRange.DateEnd, cancellationToken);

            await Task.WhenAll(revenueTask, bookingsTask, customersTask, avgValueTask, conversionTask, retentionTask);

            return new
            {
                TotalRevenue = await revenueTask,
                TotalBookings = await bookingsTask,
                TotalCustomers = await customersTask,
                AverageServiceValue = await avgValueTask,
                ConversionRate = await conversionTask,
                CustomerRetentionRate = await retentionTask
            };
        }
    }
}
