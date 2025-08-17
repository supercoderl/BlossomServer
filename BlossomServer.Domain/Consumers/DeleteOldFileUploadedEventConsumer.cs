using BlossomServer.Domain.Commands.Files.DeleteFile;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Shared.Events.FileInfo;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Consumers
{
    public sealed class DeleteOldFileUploadedEventConsumer : IConsumer<FileUploadedEvent>
    {
        private readonly IMediatorHandler _bus;
        private readonly ILogger<CreateFileUploadedEventConsumer> _logger;
        private readonly IFileInfoRepository _fileInfoRepository;

        public DeleteOldFileUploadedEventConsumer(
            IMediatorHandler bus,
            ILogger<CreateFileUploadedEventConsumer> logger,
            IFileInfoRepository fileInfoRepository
        )
        {
            _fileInfoRepository = fileInfoRepository;
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<FileUploadedEvent> context)
        {
            if (!string.IsNullOrEmpty(context.Message.OldUrl))
            {
                var fileInfo = await _fileInfoRepository.GetByUrl(context.Message.OldUrl);

                if (fileInfo != null)
                {
                    _logger.LogInformation("Deleting old file: {FileId}", fileInfo.FileId);
                    await _bus.SendCommandAsync(new DeleteFileCommand(fileInfo.FileId));
                }
                else
                {
                    _logger.LogWarning("Old file not found for URL: {OldUrl}", context.Message.OldUrl);
                }
            }
        }
    }
}
