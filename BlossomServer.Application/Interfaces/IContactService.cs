using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IContactService
    {
        public Task<PagedResult<ContactViewModel>> GetAllContactsAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task<PagedResult<ContactViewModel>> GetAllContactsByEmailAsync(
            PageQuery query,
            bool includeResponses,
            string email,
            SortQuery? sortQuery = null);

        public Task<Guid> CreateContactAsync(CreateContactViewModel viewModel);
    }
}
