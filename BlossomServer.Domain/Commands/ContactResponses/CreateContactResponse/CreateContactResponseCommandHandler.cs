using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.ContactResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ContactResponses.CreateContactResponse
{
    public sealed class CreateContactResponseCommandHandler : CommandHandlerBase, IRequestHandler<CreateContactResponseCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactResponseRepository _contactResponseRepository;

        public CreateContactResponseCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IContactRepository contactRepository,
            IContactResponseRepository contactResponseRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _contactRepository = contactRepository;
            _contactResponseRepository = contactResponseRepository;
        }

        public async Task Handle(CreateContactResponseCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var contact = await _contactRepository.GetByIdAsync(request.ContactId);

            if(contact == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no any contact with id {request.ContactId}.",
                    ErrorCodes.ObjectNotFound
                ));

                return;
            }

            var contactResponse = new Domain.Entities.ContactResponse(
                request.ContactResponseId,
                request.ContactId,
                request.ResponseText,
                request.ResponderId
            );

            _contactResponseRepository.Add(contactResponse);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ContactResponseCreatedEvent(contactResponse.Id, contact.Email, request.ResponseText));
            }
        }
    }
}
