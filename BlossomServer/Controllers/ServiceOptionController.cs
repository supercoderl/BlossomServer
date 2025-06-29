using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.ServiceOptions;
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
    public sealed class ServiceOptionController : ApiController
    {
        private readonly IServiceOptionService _serviceOptionService;

        public ServiceOptionController(
            INotificationHandler<DomainNotification> notifications,
            IServiceOptionService serviceOptionService) : base(notifications)
        {
            _serviceOptionService = serviceOptionService;
        }

        [HttpPost]
        [SwaggerOperation("Create a new service option")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> CreateServiceOptionAsync([FromBody] CreateServiceOptionViewModel viewModel)
        {
            var serviceOptionId = await _serviceOptionService.CreateServiceOptionAsync(viewModel);
            return Response(serviceOptionId);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete a service option")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> DeleteServiceAsync([FromRoute] Guid id)
        {
            await _serviceOptionService.DeleteServiceOptionAsync(id);
            return Response(id);
        }

        [HttpPut]
        [SwaggerOperation("Update a service option")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UpdateServiceOptionViewModel>))]
        public async Task<IActionResult> UpdateServiceOptionAsync([FromBody] UpdateServiceOptionViewModel viewModel)
        {
            await _serviceOptionService.UpdateServiceOptionAsync(viewModel);
            return Response(viewModel);
        }
    }
}
