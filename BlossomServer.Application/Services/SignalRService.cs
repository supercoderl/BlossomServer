using BlossomServer.Application.Hubs;
using BlossomServer.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BlossomServer.Application.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<TrackerHub> _hub;

        public SignalRService(
            IHubContext<TrackerHub> hub
        )
        {
            _hub = hub;
        }

        public async Task SendData(string type, object data, string target, string? groupId = null, string? receiverId = null)
        {
            var payload = new
            {
                type,
                data
            };

            switch (target.ToLower())
            {
                case "group":
                    if (groupId is null)
                        throw new ArgumentNullException(nameof(groupId));
                    await _hub.Clients.Group(groupId).SendAsync("receiveData", payload);
                    break;

                case "connection":
                    var connectionId = TrackerHub.GetConnectionId(receiverId);
                    if (connectionId is not null)
                        await _hub.Clients.Client(connectionId).SendAsync("receiveData", payload);
                    break;

                case "all":
                    await _hub.Clients.All.SendAsync("receiveData", payload);
                    break;

                default:
                    throw new ArgumentException("Invalid target specified.");
            }
        }
    }
}
