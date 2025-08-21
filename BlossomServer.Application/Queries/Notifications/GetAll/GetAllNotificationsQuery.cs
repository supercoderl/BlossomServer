using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Enums;
using MediatR;

namespace BlossomServer.Application.Queries.Notifications.GetAll
{
    public sealed record GetAllNotificationsQuery(
        PageQuery Query,
        bool IncludeDeleted,
        Guid ReceiverId,
        UserRole Role,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<NotificationViewModel>>;
}
