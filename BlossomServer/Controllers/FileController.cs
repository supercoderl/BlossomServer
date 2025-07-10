using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Files;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public sealed class FileController : ApiController
    {
        private readonly IFileService _fileService;

        public FileController(
            INotificationHandler<DomainNotification> notifications,
            IFileService fileService) : base(notifications)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [SwaggerOperation("Upload example")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<string>))]
        public async Task<IActionResult> UploadExampleFileAsync(
            [FromForm] UploadExampleFileViewModel viewModel
        )
        {
            var url = await _fileService.UploadExampleFile(viewModel);
            return Response(url);
        }
    }
}
