using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.DeleteUser
{
    public sealed class DeleteUserCommandHandler : CommandHandlerBase,
        IRequestHandler<DeleteUserCommand>
    {
        private readonly IUser _user;
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository,
            IUser user) : base(bus, unitOfWork, notifications)
        {
            _userRepository = userRepository;
            _user = user;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request))
            {
                return;
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                await NotifyAsync(
                    new DomainNotification(
                        request.MessageType,
                        $"There is no user with Id {request.UserId}",
                        ErrorCodes.ObjectNotFound));

                return;
            }

            _userRepository.Remove(user);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new UserDeletedEvent(request.UserId));
            }
        }
    }
}
