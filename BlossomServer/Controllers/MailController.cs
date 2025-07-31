using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Mails;
using BlossomServer.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/v1/[controller]")]
    public sealed class MailController : ApiController
    {
        private readonly IMailService _mailService;

        public MailController(
            INotificationHandler<DomainNotification> notifications,
            IMailService mailService) : base(notifications)
        {
            _mailService = mailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Send a mail")]
        [SwaggerResponse(200, "Request successful")]
        public async Task<IActionResult> SendMailAsync([FromBody] SendMailViewModel viewModel)
        {
            await _mailService.SendMailAsync(viewModel);
            return Response();
        }
    }
}
