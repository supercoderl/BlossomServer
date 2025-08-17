using BlossomServer.Domain.Commands.Files.CreateFile;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Shared.Events.FileInfo;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BlossomServer.Domain.Consumers
{
    public sealed class CreateFileUploadedEventConsumer : IConsumer<FileUploadedEvent>
    {
        private readonly IMediatorHandler _bus;
        private readonly ILogger<CreateFileUploadedEventConsumer> _logger;

        public CreateFileUploadedEventConsumer(
            IMediatorHandler bus,
            ILogger<CreateFileUploadedEventConsumer> logger
        )
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<FileUploadedEvent> context)
        {
            _logger.LogInformation("Creating new file: {FileId}", context.Message.FileId);

            await _bus.SendCommandAsync(new CreateFileCommand(
                    context.Message.AggregateId,
                    context.Message.FileId,
                    context.Message.Url,
                    context.Message.FileName
                ));
        }
    }
}
