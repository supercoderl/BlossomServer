using BlossomServer.Application.ViewModels;

namespace BlossomServer.Application.Interfaces
{
    public interface IDashboardService
    {
        public Task<object> GetBusinessAnalyticsAsync(
            PageQuery query,
            DateRange currentRange,
            DateRange previousRange
        );
    }
}
