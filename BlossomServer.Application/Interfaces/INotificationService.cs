using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface INotificationService
    {
        public Task<PagedResult<NotificationViewModel>> GetAllNotificationsAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);
    }
}
