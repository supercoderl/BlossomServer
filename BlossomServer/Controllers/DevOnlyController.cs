using BlossomServer.Application.Interfaces;
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
            await _signalRService.SendData("system", new
            {
                NotificationId = Guid.NewGuid(),
                Message = "You have new booking!!"
            }, "group", "administrators");
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
    }
}
