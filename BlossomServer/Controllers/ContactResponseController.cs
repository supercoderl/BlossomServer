using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.ContactResponses;
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
    public sealed class ContactResponseController : ApiController
    {
        private readonly IContactResponseService _contactResponseService;

        public ContactResponseController(
            INotificationHandler<DomainNotification> notifications,
            IContactResponseService contactResponseService) : base(notifications)
        {
            _contactResponseService = contactResponseService;
        }

        [HttpPost]
        [SwaggerOperation("Create a new response")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> CreateContactResponseAsync([FromBody] CreateContactResponseViewModel viewModel)
        {
            var contactResponseId = await _contactResponseService.CreateContactResponseAsync(viewModel);
            return Response(contactResponseId);
        }
    }
}
