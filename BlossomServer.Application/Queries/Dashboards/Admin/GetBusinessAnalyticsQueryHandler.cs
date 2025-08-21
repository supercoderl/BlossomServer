using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.SharedKernel.Utils;
using MediatR;

namespace BlossomServer.Application.Queries.Dashboards.Admin
{
    public sealed class GetBusinessAnalyticsQueryHandler :
                IRequestHandler<GetBusinessAnalyticsQuery, object>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITechnicianRepository _technicianRepository;

        public GetBusinessAnalyticsQueryHandler(
            IBookingRepository bookingRepository,
            IServiceRepository serviceRepository,
            ICategoryRepository categoryRepository,
            ITechnicianRepository technicianRepository
        )
        {
            _bookingRepository = bookingRepository;
            _serviceRepository = serviceRepository;
            _categoryRepository = categoryRepository;
            _technicianRepository = technicianRepository;
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
            var categoryTask = _categoryRepository.GetCategoriesWithServicesDetailSQL(cancellationToken);
            var technicianTask = _technicianRepository.GetAllTechniciansBySQL(string.Empty, false, request.Query.Page, request.Query.PageSize, string.Empty, string.Empty, cancellationToken);
            var serviceTask = _serviceRepository.GetServicesPopularityRanking(currentRange.DateStart, currentRange.DateEnd, previousRange.DateStart, previousRange.DateEnd, cancellationToken);
            var scheduleTask = _bookingRepository.GetScheduleByDateSQL(TimeZoneHelper.GetLocalTimeNow().ToString("yyyy-MM-dd"), cancellationToken);

            await Task.WhenAll(
                revenueTask, 
                bookingsTask, 
                customersTask, 
                avgValueTask, 
                conversionTask, 
                retentionTask, 
                categoryTask, 
                technicianTask, 
                serviceTask,
                scheduleTask
            );

            var technicianTaskResult = await technicianTask;

            return new
            {
                Revenue = await revenueTask,
                Bookings = await bookingsTask,
                TotalCustomers = await customersTask,
                AverageServiceValue = await avgValueTask,
                ConversionRate = await conversionTask,
                CustomerRetentionRate = await retentionTask,
                categories = await categoryTask,
                technicians = technicianTaskResult.Select(t => new
                {
                    t.Id,
                    t.UserId,
                    Fullname = t.User?.FullName ?? string.Empty,
                    AvatarUrl = t.User?.AvatarUrl ?? string.Empty,
                    t.YearsOfExperience
                }).ToList(),
                Services = await serviceTask,
                Schedules = await scheduleTask
            };
        }
    }
}
