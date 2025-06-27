using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace BlossomServer.Domain.Commands.Files.UploadFile
{
    public sealed class UploadFileCommandHandler : CommandHandlerBase, IRequestHandler<UploadFileCommand, string>
    {
        private readonly HttpClient _http;
        private readonly BunnyCDNSettings _settings;

        public UploadFileCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IHttpClientFactory httpClientFactory,
            IOptions<BunnyCDNSettings> options
        ) : base(bus, unitOfWork, notifications)
        {
            _http = httpClientFactory.CreateClient();
            _settings = options.Value;
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return string.Empty;

            var fileName = Path.GetFileName(request.File.FileName);
            var destinationPath = $"{_settings.UploadPath}/{fileName}";
            string baseUrl = string.IsNullOrEmpty(_settings.Region) ? "https://storage.bunnycdn.com" : $"https://{_settings.Region}.storage.bunnycdn.com";

            var url = $"{baseUrl}/{_settings.StorageZoneName}/{destinationPath.TrimStart('/')}";

            using var stream = request.File.OpenReadStream();

            var rqs = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StreamContent(stream)
            };

            rqs.Headers.Add("AccessKey", _settings.AccessKey);

            if (!string.IsNullOrEmpty(request.ContentTypeOverride))
                rqs.Headers.Add("Override-Content-Type", request.ContentTypeOverride);

            if (request.ValidateChecksum && stream.CanSeek)
            {
                long originalPos = stream.Position;
                using var sha = SHA256.Create();
                var hashBytes = sha.ComputeHash(stream);
                stream.Position = originalPos;

                var checksum = Convert.ToHexString(hashBytes).ToLower();
                rqs.Headers.Add("Checksum", checksum);
            }

            var rps = await _http.SendAsync(rqs);
            if (!rps.IsSuccessStatusCode)
            {
                var msg = await rps.Content.ReadAsStringAsync();
                await NotifyAsync(
                    new DomainNotification(
                        "UploadFile",
                        $"Failed to upload file: {msg}",
                        ErrorCodes.UploadFailed
                    )
                );

                return string.Empty;
            }

            return $"https://{_settings.StorageZoneName}.b-cdn.net/{Uri.EscapeDataString(destinationPath)}";
        }
    }
}
