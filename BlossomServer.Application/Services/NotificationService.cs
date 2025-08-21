using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Notifications.GetAll;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public NotificationService(IMediatorHandler bus, IUser user)
        {
            _bus = bus;
            _user = user;
        }

        public async Task<PagedResult<NotificationViewModel>> GetAllNotificationsAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllNotificationsQuery(
                query, 
                includeDeleted, 
                _user.GetUserId(), 
                _user.GetUserRole(),
                searchTerm, 
                sortQuery
            ));
        }
    }
}
