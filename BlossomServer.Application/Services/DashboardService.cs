using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Dashboards.Admin;
using BlossomServer.Application.ViewModels;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMediatorHandler _bus;

        public DashboardService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<object> GetBusinessAnalyticsAsync(PageQuery query, DateRange currentRange, DateRange previousRange)
        {
            return await _bus.QueryAsync(new GetBusinessAnalyticsQuery(query, currentRange, previousRange));
        }
    }
}
