using BlossomServer.Application.Queries.Promotions.GetById;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.Messages.CheckBot
{
    public sealed class CheckBotQueryHandler :
            IRequestHandler<CheckBotQuery, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediatorHandler _bus;

        public CheckBotQueryHandler(IUserRepository userRepository, IMediatorHandler bus)
        {
            _userRepository = userRepository;
            _bus = bus;
        }

        public async Task<bool> Handle(CheckBotQuery request, CancellationToken cancellationToken)
        {
            var bot = await _userRepository.GetByIdAsync(request.Id);

            if (bot is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetPromotionByIdQuery),
                        $"User with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return false;
            }

            return bot.IsBot;
        }
    }
}
