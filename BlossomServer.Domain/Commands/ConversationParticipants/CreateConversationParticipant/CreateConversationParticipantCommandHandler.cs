using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.ConversationParticipant;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ConversationParticipants.CreateConversationParticipant
{
    public sealed class CreateConversationParticipantCommandHandler : CommandHandlerBase, IRequestHandler<CreateConversationParticipantCommand>
    {
        private readonly IConversationParticipantRepository _conversationParticipantRepository;

        public CreateConversationParticipantCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IConversationParticipantRepository conversationParticipantRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _conversationParticipantRepository = conversationParticipantRepository;
        }

        public async Task Handle(CreateConversationParticipantCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var conversationParticipant = new Entities.ConversationParticipant(
                request.ConversationParticipantId,
                request.ConversationId,
                request.UserId
            );

            _conversationParticipantRepository.Add( conversationParticipant );

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ConversationParticipantCreatedEvent(conversationParticipant.Id));
            }
        }
    }
}
