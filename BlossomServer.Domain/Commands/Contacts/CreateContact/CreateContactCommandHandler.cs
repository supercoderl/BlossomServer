using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Contact;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Contacts.CreateContact
{
    public sealed class CreateContactCommandHandler : CommandHandlerBase, IRequestHandler<CreateContactCommand>
    {
        private readonly IContactRepository _contactRepository;

        public CreateContactCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IContactRepository contactRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _contactRepository = contactRepository;
        }

        public async Task Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var contact = new Entities.Contact(
                request.ContactId,
                request.Name,
                request.Email,
                request.Message
            );

            _contactRepository.Add(contact);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ContactCreatedEvent(contact.Id));
            }
        }
    }
}
