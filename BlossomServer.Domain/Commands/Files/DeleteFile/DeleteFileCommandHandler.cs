using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using Imagekit.Sdk;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.DeleteFile
{
    public sealed class DeleteFileCommandHandler : CommandHandlerBase, IRequestHandler<DeleteFileCommand>
    {
        private readonly ImageKitSettings _settings;
        private readonly ImagekitClient _imagekitClient;

        public DeleteFileCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notification,
            IOptions<ImageKitSettings> options
        ) : base(bus, unitOfWork, notification)
        {
            _settings = options.Value;
            _imagekitClient = new ImagekitClient(_settings.PublicKey, _settings.PrivateKey, _settings.EndPoint);
        }

        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            ResultDelete resp = _imagekitClient.DeleteFile(request.FileId);
        }
    }
}
