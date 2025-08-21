using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Helpers;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using Microsoft.Extensions.Options;
using BC = BCrypt.Net.BCrypt;

namespace BlossomServer.Domain.Commands.Users.Login
{
    public sealed class LoginUserCommandHandler : CommandHandlerBase,
        IRequestHandler<LoginUserCommand, object>
    {
        private readonly TokenSettings _tokenSettings;

        private readonly IUserRepository _userRepository;

        public LoginUserCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository,
            IOptions<TokenSettings> tokenSettings) : base(bus, unitOfWork, notifications)
        {
            _userRepository = userRepository;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<object> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request))
            {
                return new { };
            }

            var user = await _userRepository.GetUserByIdentifierAsync(request.Identifier);

            if (user is null)
            {
                await NotifyAsync(
                    new DomainNotification(
                        request.MessageType,
                        $"There is no user with identifier {request.Identifier}",
                        ErrorCodes.ObjectNotFound));

                return new { };
            }

            var passwordVerified = BC.Verify(request.Password, user.Password);

            if (!passwordVerified)
            {
                await NotifyAsync(
                    new DomainNotification(
                        request.MessageType,
                        "The password is incorrect",
                        DomainErrorCodes.User.PasswordIncorrect));

                return new { };
            }

            user.SetActive();
            user.SetLastLoggedIn(TimeZoneHelper.ConvertUtcToLocal(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")));

            var refreshToken = await TokenHelper.GenerateRefreshToken(user.Id, Bus);

            if (!await CommitAsync())
            {
                return new { };
            }

            return new
            {
                AccessToken = TokenHelper.BuildToken(user, _tokenSettings),
                RefreshToken = refreshToken,
                UserInfo = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    phoneNumber = user.PhoneNumber,
                    role = user.Role.ToString(),
                    avatarUrl = user.AvatarUrl,
                    lastLogin = user.LastLoggedinDate
                }
            };
        }
    }
}
