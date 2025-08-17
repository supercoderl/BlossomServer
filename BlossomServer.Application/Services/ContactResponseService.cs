using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.ContactResponses;
using BlossomServer.Domain.Commands.ContactResponses.CreateContactResponse;
using BlossomServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Services
{
    public class ContactResponseService : IContactResponseService
    {
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public ContactResponseService(IMediatorHandler bus, IUser user)
        {
            _bus = bus;
            _user = user;
        }

        public async Task<Guid> CreateContactResponseAsync(CreateContactResponseViewModel viewModel)
        {
            var id = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateContactResponseCommand(id, viewModel.ContactId, viewModel.ResponseText, _user.GetUserId()));

            return id;
        }
    }
}
