using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Subscribers;
using BlossomServer.Domain.Commands.Subscribers.Subscribe;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IMediatorHandler _bus;

        public SubscriberService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> SubscribeAsync(SubscribeViewModel viewModel)
        {
            var id = Guid.NewGuid();

            await _bus.SendCommandAsync(new SubscribeCommand(id, viewModel.Email));

            return id;
        }
    }
}
