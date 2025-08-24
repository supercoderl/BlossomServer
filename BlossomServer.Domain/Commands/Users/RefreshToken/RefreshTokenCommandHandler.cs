using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Helpers;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.RefreshToken
{
    public sealed class RefreshTokenCommandHandler : CommandHandlerBase, IRequestHandler<RefreshTokenCommand, object>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly TokenSettings _tokenSettings;

        public RefreshTokenCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IOptions<TokenSettings> tokenSettings
        ) : base(bus, unitOfWork, notifications)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<object> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return string.Empty;

            var refreshToken = await _refreshTokenRepository.GetByToken(request.RefreshToken);

            if(refreshToken == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"Token not found!",
                    ErrorCodes.ObjectNotFound
                ));
                return string.Empty;
            }

            if(refreshToken.ExpiryDate <= TimeZoneHelper.GetLocalTimeNow())
            {
                await NotifyAsync(new DomainNotification(
                     request.MessageType,
                     $"Token was expired!",
                     ErrorCodes.ExpiredToken
                 ));
                return string.Empty;
            }

            refreshToken.SetExpiryDate(TimeZoneHelper.GetLocalTimeNow());

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId);

            if (user == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"User not found!",
                    ErrorCodes.ObjectNotFound
                ));
                return string.Empty;
            }

            await CommitAsync();

            var newRefreshToken = await TokenHelper.GenerateRefreshToken(user.Id, Bus, _tokenSettings);

            return new
            {
                AccessToken = TokenHelper.BuildToken(user, _tokenSettings),
                RefreshToken = newRefreshToken,
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
