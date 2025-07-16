using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BlossomServer.Application.Hubs
{
    [Authorize]
    public sealed class TrackerHub : Hub
    {
        private readonly IMediatorHandler _bus;
        private readonly IUserRepository _userRepository;
        public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        public TrackerHub(
            IMediatorHandler bus,
            IUserRepository userRepository
        )
        {
            _bus = bus;
            _userRepository = userRepository;
        }

        public async Task SendStructuredData(string type, object payload)
        {
            try
            {
                var sender = _Connections.FirstOrDefault(u => u.Id == Guid.Parse(UserId));
                if (sender is null || sender.CurrentRoomId is null)
                    throw new HubException("Sender not in a group.");

                var parsedType = type.ToLowerInvariant();

                // Tự map type -> enum nếu bạn muốn enforce
                if (!Enum.TryParse<MessagePayloadType>(parsedType, true, out var payloadType))
                    throw new HubException("Invalid payload type.");

                // Nếu là chat thì tạo viewmodel và ghi DB
                if (payloadType == MessagePayloadType.Chat || payloadType == MessagePayloadType.SystemMessage)
                {
                    /*                    var msg = payload.ToString();

                                        var viewModel = new CreateMessageViewModel(
                                            sender.CurrentRoomId.Value,
                                            msg,
                                            DateTime.UtcNow,
                                            Guid.Parse(_userId),
                                            false,
                                            payloadType == MessagePayloadType.SystemMessage
                                        );

                                        await HubHelper.SendStructuredPayload(
                                            _bus,
                                            Clients.OthersInGroup(sender.CurrentRoomId.Value.ToString()),
                                            payloadType,
                                            viewModel,
                                            _userId,
                                            sender.CurrentRoomId
                                        );*/
                }
                else
                {
                    // Các loại khác: chỉ gửi thôi, không ghi DB
                    await SendData(type, payload, "othersingroup", groupId: sender.CurrentRoomId.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                await SendData("error", new { message = ex.Message }, "caller");
            }
        }

        private async Task SendData(string type, object data, string target, string? groupId = null, string? connectionId = null)
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
                    await Clients.Group(groupId).SendAsync("receiveData", payload);
                    break;

                case "othersingroup":
                    if (groupId is null)
                        throw new ArgumentNullException(nameof(groupId));
                    await Clients.OthersInGroup(groupId).SendAsync("receiveData", payload);
                    break;

                case "caller":
                    await Clients.Caller.SendAsync("receiveData", payload);
                    break;

                case "connection":
                    if (connectionId is null)
                        throw new ArgumentNullException(nameof(connectionId));
                    await Clients.Client(connectionId).SendAsync("receiveData", payload);
                    break;

                case "all":
                    await Clients.All.SendAsync("receiveData", payload);
                    break;

                default:
                    throw new ArgumentException("Invalid target specified.");
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(Guid.Parse(UserId));

                if (user is null)
                    throw new HubException("User not found.");

                var userViewModel = UserViewModel.FromUser(
                    user,
                    GetDevice(),
                    null,
                    Context.ConnectionId
                );

                lock (_Connections)
                {
                    // Remove existing connection if any (handles reconnection scenarios)
                    var existingUser = _Connections.FirstOrDefault(u => u.Id == Guid.Parse(UserId));
                    if (existingUser != null)
                    {
                        _Connections.Remove(existingUser);
                    }

                    _Connections.Add(userViewModel);
                    _ConnectionsMap[UserId] = Context.ConnectionId;
                }

                await Clients.Caller.SendAsync("getProfileInfo", userViewModel);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }

            await base.OnConnectedAsync(); // Fixed: Removed Task<Task> return type and added await
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var user = _Connections.FirstOrDefault(u => u.Id == Guid.Parse(UserId)); // Fixed: Added missing _userId

                // Tell other users to remove you from their list
                if (user is not null)
                {
                    if (user.CurrentRoomId.HasValue)
                    {
                        await Task.WhenAll(
                            Groups.RemoveFromGroupAsync(Context.ConnectionId, user.CurrentRoomId.Value.ToString()),
                            Clients.OthersInGroup(user.CurrentRoomId.Value.ToString()).SendAsync("removeUser", user)
                        );
                    }

                    lock (_Connections)
                    {
                        _Connections.Remove(user);

                        // Remove mapping
                        _ConnectionsMap.Remove(user.Id.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            await base.OnDisconnectedAsync(exception); // Fixed: Removed Task<Task> return type and added await
        }

        private string GetDevice()
        {
            var device = Context.GetHttpContext()?.Request.Headers["Device"].ToString();
            return !string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")) ? device : "Web";
        }

        public async Task JoinGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            await Clients.Caller.SendAsync("joinedGroup", groupId);
        }

        public async Task LeaveGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task JoinDashboardGroup(Domain.Enums.UserRole role)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, role.ToString());
        }

        public async Task LeaveDashboardGroup(Domain.Enums.UserRole role)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, role.ToString());
        }

        private string UserId
        {
            get
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    throw new HubException("You have not signed in.");
                }
                return userId;
            }
        }
    }
}