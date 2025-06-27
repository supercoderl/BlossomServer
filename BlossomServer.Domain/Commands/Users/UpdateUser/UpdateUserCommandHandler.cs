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
using BlossomServer.Domain.Commands.Files.UploadFile;

namespace BlossomServer.Domain.Commands.Users.UpdateUser
{
    public sealed class UpdateUserCommandHandler : CommandHandlerBase,
        IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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

            if (request.Email != user.Email)
            {
                var userWithSameEmail = await _userRepository.GetUserByIdentifierAsync(request.Email);
                if (userWithSameEmail is not null && userWithSameEmail.Id != user.Id)
                {
                    await NotifyAsync(
                        new DomainNotification(
                            request.MessageType,
                            $"There is already a user with email {request.Email}",
                            DomainErrorCodes.User.AlreadyExists));
                    return;
                }
            }

            user.SetEmail(request.Email);
            user.SetFirstName(request.FirstName);
            user.SetLastName(request.LastName);
            user.SetPhoneNumber(request.PhoneNumber);

            if(request.AvatarFile != null)
            {
                string url = await Bus.QueryAsync(new UploadFileCommand(
                    request.AvatarFile,
                    null,
                    false
                ));
                user.SetAvatarUrl(url);
            }

            user.SetGender(request.Gender);
            user.SetDateOfBirth(request.DateOfBirth);
            user.SetRole(request.Role);

            _userRepository.Update(user);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new UserUpdatedEvent(user.Id));
            }
        }
    }
}
