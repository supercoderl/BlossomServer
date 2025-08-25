using BlossomServer.Application.Hubs;
using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public sealed class DevOnlyController : ApiController
    {
        private readonly ISignalRService _signalRService;
        private readonly IMessageService _messageService;

        public DevOnlyController(
            INotificationHandler<DomainNotification> notifications,
            ISignalRService signalRService,
            IMessageService messageService
        ) : base(notifications)
        {
            _signalRService = signalRService;
            _messageService = messageService;
        }

        [HttpPost]
        [SwaggerOperation("Test send admin notification")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<object>))]
        public async Task<IActionResult> TestAdminNotificationAsync()
        {
            await _signalRService.SendData("system", NotificationViewModel.FromNotification(
                new Domain.Entities.Notification(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Booking Arrived",
                    "You have a new booking!",
                    Domain.Enums.NotificationType.NewBooking,
                    0,
                    null,
                    null,
                    null
                )), 
            "group", "administrators");
            return Response();
        }

        [HttpPost("message")]
        [SwaggerOperation("Test send message to AI")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<string>))]
        public async Task<IActionResult> TestSendMessageAsync([FromBody] string content)
        {
            var result = await _messageService.SendPromptAsync(content);
            return Response(result);
        }

        [HttpGet("connections")]
        [SwaggerOperation("Test get connections")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Dictionary<string, string>>))]
        public async Task<IActionResult> TestGetConnectionAsync()
        {
            await Task.CompletedTask;
            var result = TrackerHub.GetConnections();
            return Response(result);
        }

        [HttpPost("test-cookie")]
        public IActionResult TestCookie()
        {
            HttpContext.Response.Cookies.Append("test_cookie", "test_value", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok("Cookie set");
        }

        [HttpGet("read-cookie")]
        public IActionResult ReadCookie()
        {
            var testCookie = Request.Cookies["test_cookie"];
            var accessToken = Request.Cookies["access_token"];

            return Ok(new { testCookie, accessToken });
        }
    }
}
