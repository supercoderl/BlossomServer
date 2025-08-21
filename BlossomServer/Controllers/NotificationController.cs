using BlossomServer.Application.Interfaces;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using BlossomServer.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/v1/[controller]")]
    public sealed class NotificationController : ApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(
            INotificationHandler<DomainNotification> notifications,
            INotificationService notificationService) : base(notifications)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Get a list of all notifications")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<NotificationViewModel>>))]
        public async Task<IActionResult> GetAllNotificationsAsync(
            [FromQuery] PageQuery query,
            [FromQuery] string searchTerm = "",
            [FromQuery] bool includeDeleted = false,
            [FromQuery] [SortableFieldsAttribute<NotificationViewModelSortProvider,NotificationViewModel, Notification>]
        SortQuery? sortQuery = null)
        {
            var notifications = await _notificationService.GetAllNotificationsAsync(
                query,
                includeDeleted,
                searchTerm,
                sortQuery);
            return Response(notifications);
        }
    }
}
