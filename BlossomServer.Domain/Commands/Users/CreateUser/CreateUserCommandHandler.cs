using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using BlossomServer.Shared.Events.User;
using BlossomServer.Domain.Commands.Technicians.CreateTechnician;
using BlossomServer.SharedKernel.Utils;

namespace BlossomServer.Domain.Commands.Users.CreateUser
{
    public sealed class CreateUserCommandHandler : CommandHandlerBase,
        IRequestHandler<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request))
            {
                return;
            }

            var existingUser = await _userRepository.GetByIdAsync(request.UserId);

            if (existingUser is not null)
            {
                await NotifyAsync(
                    new DomainNotification(
                        request.MessageType,
                        $"There is already a user with Id {request.UserId}",
                        DomainErrorCodes.User.AlreadyExists));
                return;
            }

            existingUser = await _userRepository.GetUserByIdentifierAsync(request.Email);

            if (existingUser is not null)
            {
                await NotifyAsync(
                    new DomainNotification(
                        request.MessageType,
                        $"There is already a user with email {request.Email}",
                        DomainErrorCodes.User.AlreadyExists));
                return;
            }

            var passwordHash = BC.HashPassword(request.Password);

            var user = new User(
                request.UserId,
                passwordHash,
                request.FirstName,
                request.LastName,
                TextHelper.NomalizeGmail(request.Email),
                request.PhoneNumber,
                request.AvatarUrl,
                request.CoverPhotoUrl,
                request.Gender,
                request.Website,
                request.DateOfBirth,
                request.Role
            );

            _userRepository.Add(user);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new UserCreatedEvent(user.Id));
                if(user.Role == Enums.UserRole.Technician)
                {
                    await Bus.SendCommandAsync(new CreateTechnicianCommand(
                        Guid.NewGuid(),
                        user.Id,
                        string.Empty,
                        0,
                        0
                    ));
                }
            }
        }
    }
}
