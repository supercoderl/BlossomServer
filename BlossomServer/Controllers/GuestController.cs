using BlossomServer.Application.Interfaces;
using BlossomServer.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlossomServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GuestController : ApiController
    {
        private readonly IUserService _userService;

        public GuestController(IUserService userService, INotificationHandler<DomainNotification> notification) : base(notification)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateGuestTokenAsync(
            [FromQuery] string clientId
        )
        {
            var token = await _userService.GenerateGuestTokenAsync( 
                clientId,
                HttpContext.Request.Headers["User-Agent"].ToString() 
            );

            return Response(token);
        }
    }
}
