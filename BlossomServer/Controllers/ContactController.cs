using BlossomServer.Application.Interfaces;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Contacts;
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
    public sealed class ContactController : ApiController
    {
        private readonly IContactService _contactService;

        public ContactController(
            INotificationHandler<DomainNotification> notifications,
            IContactService contactService) : base(notifications)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Get a list of all contacts")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<ContactViewModel>>))]
        public async Task<IActionResult> GetAllContactsAsync(
            [FromQuery] PageQuery query,
            [FromQuery] string searchTerm = "",
            [FromQuery] bool includeDeleted = false,
            [FromQuery] [SortableFieldsAttribute<ContactViewModelSortProvider, ContactViewModel, Contact>]
        SortQuery? sortQuery = null)
        {
            var contacts = await _contactService.GetAllContactsAsync(
                query,
                includeDeleted,
                searchTerm,
                sortQuery);
            return Response(contacts);
        }

        [HttpGet("with-email")]
        [AllowAnonymous]
        [SwaggerOperation("Get a list of all contacts with email")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<ContactViewModel>>))]
        public async Task<IActionResult> GetAllContactsByEmailAsync(
            [FromQuery] PageQuery query,
            [FromQuery] string email,
            [FromQuery] bool includeResponses = false,
            [FromQuery] [SortableFieldsAttribute<ContactViewModelSortProvider, ContactViewModel, Contact>]
        SortQuery? sortQuery = null)
        {
            var contacts = await _contactService.GetAllContactsByEmailAsync(
                query,
                includeResponses,
                email,
                sortQuery);
            return Response(contacts);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Create a new contact")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> CreateContactAsync([FromBody] CreateContactViewModel viewModel)
        {
            var contactId = await _contactService.CreateContactAsync(viewModel);
            return Response(contactId);
        }
    }
}
