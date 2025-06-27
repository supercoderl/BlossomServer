using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.RefreshToken;
using MediatR;

namespace BlossomServer.Domain.Commands.RefreshToken.CreateRefreshToken
{
    public sealed class CreateRefreshTokenCommandHandler : CommandHandlerBase, IRequestHandler<CreateRefreshTokenCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public CreateRefreshTokenCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IRefreshTokenRepository refreshTokenRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var refreshToken = new Entities.RefreshToken(
                request.RefreshTokenId,
                request.UserId,
                request.Token,
                request.ExpiryDate
            );

            _refreshTokenRepository.Add(refreshToken);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new RefreshTokenCreatedEvent(refreshToken.Id));
            }
        }
    }
}
