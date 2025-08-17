using BlossomServer.Application.ViewModels.ContactResponses;

namespace BlossomServer.Application.Interfaces
{
    public interface IContactResponseService
    {
        public Task<Guid> CreateContactResponseAsync(CreateContactResponseViewModel viewModel);
    }
}
