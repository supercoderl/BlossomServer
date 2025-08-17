using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Contacts.GetAll;
using BlossomServer.Application.Queries.Contacts.GetAllByEmail;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Contacts.CreateContact;
using BlossomServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IMediatorHandler _bus;

        public ContactService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> CreateContactAsync(CreateContactViewModel viewModel)
        {
            var id = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateContactCommand(id, viewModel.Name, viewModel.Email, viewModel.Message));

            return id;
        }

        public async Task<PagedResult<ContactViewModel>> GetAllContactsAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllContactsQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<PagedResult<ContactViewModel>> GetAllContactsByEmailAsync(PageQuery query, bool includeResponses, string email, SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllContactsByEmailQuery(query, includeResponses, email, sortQuery));
        }
    }
}
