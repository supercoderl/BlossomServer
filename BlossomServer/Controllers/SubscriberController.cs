using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Subscribers;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public sealed class SubscriberController : ApiController
    {
        private readonly ISubscriberService _subscriberService;

        public SubscriberController(
            INotificationHandler<DomainNotification> notifications,
            ISubscriberService subscriberService) : base(notifications)
        {
            _subscriberService = subscriberService;
        }

        [HttpPost]
        [SwaggerOperation("Subcribe")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> SubscribeAsync([FromBody] SubscribeViewModel viewModel)
        {
            var subscriberId = await _subscriberService.SubscribeAsync(viewModel);
            return Response(subscriberId);
        }
    }
}
