using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using BlossomServer.Shared.Events.FileInfo;
using BlossomServer.SharedKernel.Utils;
using Imagekit.Sdk;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlossomServer.Domain.Commands.Files.UploadFile
{
    public sealed class UploadFileCommandHandler : CommandHandlerBase, IRequestHandler<UploadFileCommand, string>
    {
        private readonly HttpClient _http;
        private readonly ImageKitSettings _settings;
        private readonly ImagekitClient _imagekitClient;

        public UploadFileCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IHttpClientFactory httpClientFactory,
            IOptions<ImageKitSettings> options
        ) : base(bus, unitOfWork, notifications)
        {
            _http = httpClientFactory.CreateClient();
            _settings = options.Value;
            _imagekitClient = new ImagekitClient(_settings.PublicKey, _settings.PrivateKey, _settings.EndPoint);
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return string.Empty;

            string base64ImageRepresentation = await FileHelper.ConvertFileToByte(request.File);

            // Upload by Base64
            FileCreateRequest ob2 = new FileCreateRequest
            {
                file = base64ImageRepresentation,
                fileName = request.File.FileName
            };

            Result resp = _imagekitClient.Upload(ob2);

            await Bus.RaiseEventAsync(new FileUploadedEvent(
                Guid.NewGuid(), 
                resp.fileId, 
                request.OldUrl,
                resp.url, 
                ob2.fileName
            ));

            return resp.url;
        }
    }
}
