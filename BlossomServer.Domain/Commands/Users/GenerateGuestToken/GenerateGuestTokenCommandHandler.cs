using BlossomServer.Domain.Helpers;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using BlossomServer.SharedKernel.Utils;
using MassTransit.Futures.Contracts;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.GenerateGuestToken
{
    public sealed class GenerateGuestTokenCommandHandler : CommandHandlerBase, IRequestHandler<GenerateGuestTokenCommand, string>
    {
        private readonly TokenSettings _tokenSettings;

        public GenerateGuestTokenCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IOptions<TokenSettings> tokenSettings
        ) : base(bus, unitOfWork, notifications )
        {
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<string> Handle(GenerateGuestTokenCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return string.Empty;

            Guid guestGuid = Guid.NewGuid();

            return TokenHelper.BuildToken(
                new Entities.User(
                    guestGuid,
                    string.Empty,
                    "Guest",
                    string.Empty,
                    $"guest-{guestGuid.ToString().Substring(0, 6)}@blossomnails.com",
                    string.Empty,
                    "https://cdn1.iconfinder.com/data/icons/emoticon-of-avatar-woman/128/08_woman_mocking_avatar_emoticon_smiley_people_user-512.png",
                    null,
                    Enums.Gender.Unknow,
                    null,
                    DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                    Enums.UserRole.Guest,
                    Enums.UserStatus.Active
                ), 
                _tokenSettings
            );
        }
    }
}
