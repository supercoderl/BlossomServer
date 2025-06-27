using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.User;
using MediatR;
using BC = BCrypt.Net.BCrypt;

namespace BlossomServer.Domain.Commands.Users.ChangePassword
{
    public sealed class ChangePasswordCommandHandler : CommandHandlerBase, IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUser _user;

        public ChangePasswordCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository,
            IUser user
        ) : base(bus, unitOfWork, notifications)
        {
            _userRepository = userRepository;
            _user = user;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var user = await _userRepository.GetByIdAsync(_user.GetUserId());

            if (user == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "User not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            if(BC.Verify(request.OldPassword, user.Password) == false)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Old password is incorrect.",
                    ErrorCodes.InvalidPassword
                ));
                return;
            }

            user.SetPassword(BC.HashPassword(request.NewPassword));

            _userRepository.Update(user);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new PasswordChangedEvent(user.Id));
            }
        }
    }
}
