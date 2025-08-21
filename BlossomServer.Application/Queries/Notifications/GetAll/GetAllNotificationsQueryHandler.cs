using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Notifications.GetAll
{
    public sealed class GetAllNotificationsQueryHandler :
                IRequestHandler<GetAllNotificationsQuery, PagedResult<NotificationViewModel>>
    {
        private readonly ISortingExpressionProvider<NotificationViewModel, Notification> _sortingExpressionProvider;
        private readonly INotificationRepository _notificationRepository;

        public GetAllNotificationsQueryHandler(
            INotificationRepository notificationRepository,
            ISortingExpressionProvider<NotificationViewModel, Notification> sortingExpressionProvider)
        {
            _notificationRepository = notificationRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<NotificationViewModel>> Handle(
            GetAllNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _notificationRepository.GetAllNotificationsBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.ReceiverId,
                request.Role,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var notifications = results.Select(n => NotificationViewModel.FromNotification(n)).ToList();

            return new PagedResult<NotificationViewModel>(results.Count(), notifications, request.Query.Page, request.Query.PageSize);
        }
    }
}
