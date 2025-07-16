using BlossomServer.Application.Hubs;
using BlossomServer.Application.Interfaces;
using BlossomServer.Shared.Events.ServiceImage;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BlossomServer.Application.EventHandler
{
    public sealed class ServiceImageEventHandler :
            INotificationHandler<ServiceImageCreatedEvent>,
            INotificationHandler<ServiceImageUploadProgressEvent>
    {
        private readonly ISignalRService _signalRService;

        public ServiceImageEventHandler(ISignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task Handle(ServiceImageUploadProgressEvent notification, CancellationToken cancellationToken)
        {
            await _signalRService.SendData("system", new
            {
                ServiceId = notification.AggregateId,
                Progress = notification.ProgressPercentage,
                CurrentFile = notification.CurrentFile,
                TotalFiles = notification.TotalFiles,
                CurrentFileName = notification.CurrentFileName
            }, "group", $"service-{notification.AggregateId}", null);
        }

        public async Task Handle(ServiceImageCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _signalRService.SendData("notification", new
            {
                ServiceId = notification.AggregateId,
                Message = "All images uploaded successfully"
            }, "group", $"service-{notification.AggregateId}", null);
        }
    }
}
