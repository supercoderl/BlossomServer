using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Dashboards;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/v1/[controller]")]
    public sealed class DashboardController : ApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(
            INotificationHandler<DomainNotification> notifications,
            IDashboardService dashboardService) : base(notifications)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost("analytics")]
        [SwaggerOperation("Get business analytics dashboard")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<object>))]
        public async Task<IActionResult> GetBusinessAnalyticsAsync(
            [FromBody] BusinessAnalyticsRequest request
        )
        {
            var result = await _dashboardService.GetBusinessAnalyticsAsync(request.Query, request.CurrentRange, request.PreviousRange);
            return Response(result);
        }
    }
}
