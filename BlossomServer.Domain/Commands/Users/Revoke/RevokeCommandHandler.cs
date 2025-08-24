using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.Revoke
{
    public sealed class RevokeCommandHandler : CommandHandlerBase, IRequestHandler<RevokeCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IRefreshTokenRepository refreshTokenRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var token = await _refreshTokenRepository.GetByToken(request.RefreshToken);

            if(token != null)
            {
                token.SetExpiryDate(TimeZoneHelper.GetLocalTimeNow());

                _refreshTokenRepository.Update(token);
            }

            await CommitAsync();
        }
    }
}
