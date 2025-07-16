using BlossomServer.Application.Interfaces;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Messages;
using BlossomServer.Application.ViewModels.Services;
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
    public sealed class MessageController : ApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(
            INotificationHandler<DomainNotification> notifications,
            IMessageService messageService) : base(notifications)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [SwaggerOperation("Get a list of all messages")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<MessageViewModel>>))]
        public async Task<IActionResult> GetAllMessagesAsync(
            [FromQuery] PageQuery query,
            [FromQuery] Guid conversationId,
            [FromQuery] string searchTerm = "",
            [FromQuery] bool includeDeleted = false,
            [FromQuery] [SortableFieldsAttribute<ServiceViewModelSortProvider, ServiceViewModel, Service>]
        SortQuery? sortQuery = null)
        {
            var messages = await _messageService.GetAllMessagesAsync(
                query,
                includeDeleted,
                conversationId,
                searchTerm,
                sortQuery);
            return Response(messages);
        }

        /*[AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation("Get a service by id")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<ServiceViewModel>))]
        public async Task<IActionResult> GetServiceByIdAsync([FromRoute] Guid id)
        {
            var service = await _serviceService.GetServiceByServiceIdAsync(id);
            return Response(service);
        }*/

        [HttpGet("conversation")]
        [SwaggerOperation("Get a conversation by id")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> FindConversationIdByParticipantsAsync([FromQuery] Guid recipientId)
        {
            var id = await _messageService.FindConversationIdByParticipantsAsync(recipientId);
            return Response(id);
        }

        [HttpPost]
        [SwaggerOperation("Send a message")]
        [SwaggerResponse(200, "Request successful")]
        public async Task<IActionResult> SendMessageAsync([FromBody] CreateMessageViewModel viewModel)
        {
            await _messageService.SendMessageAsync(viewModel);
            return Response();
        }

        [HttpDelete]
        [SwaggerOperation("Delete a message or a list of messages")]
        [SwaggerResponse(200, "Request successful")]
        public async Task<IActionResult> DeleteMessageAsync([FromQuery] Guid? id, [FromQuery] Guid? conversationId)
        {
            await _messageService.DeleteMessage(id, conversationId);
            return Response();
        }

        /*[HttpPut]
        [SwaggerOperation("Update a service")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UpdateServiceViewModel>))]
        public async Task<IActionResult> UpdateServiceAsync([FromForm] UpdateServiceViewModel viewModel)
        {
            await _serviceService.UpdateServiceAsync(viewModel);
            return Response(viewModel);
        }*/
    }
}
