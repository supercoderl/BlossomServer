using BlossomServer.Application.ViewModels.Subscribers;

namespace BlossomServer.Application.Interfaces
{
    public interface ISubscriberService
    {
        public Task<Guid> SubscribeAsync(SubscribeViewModel viewModel);
    }
}
