using BlossomServer.Application.Interfaces;
using BlossomServer.Shared.Events.Admin;
using BlossomServer.Shared.Events.ServiceImage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.EventHandler
{
    public sealed class AdminNotificationRequiredEventHandler :
               INotificationHandler<AdminNotificationRequiredEvent>
    {
        private readonly ISignalRService _signalRService;

        public AdminNotificationRequiredEventHandler(ISignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task Handle(AdminNotificationRequiredEvent notification, CancellationToken cancellationToken)
        {
            await _signalRService.SendData("system", new
            {
                NotificationId = notification.AggregateId,
                Message = notification.Message
            }, "group", "administrators");
        }
    }
}
