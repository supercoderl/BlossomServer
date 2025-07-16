using BlossomServer.Domain.Commands.ConversationParticipants.CreateConversationParticipant;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Conversation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Conversations.CreateConversation
{
    public sealed class CreateConversationCommandHandler : CommandHandlerBase, IRequestHandler<CreateConversationCommand>
    {
        private readonly IConversationRepository _conversationRepository;

        public CreateConversationCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IConversationRepository conversationRepository
        ) : base(bus, unitOfWork, notifications) 
        {
            _conversationRepository = conversationRepository;
        }

        public async Task Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var conversation = new Entities.Conversation(
                request.ConversationId,
                request.Name,
                request.ConversationType,
                request.CreatedBy,
                null
            );

            _conversationRepository.Add( conversation );

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ConversationCreatedEvent(conversation.Id));
                await Bus.SendCommandAsync(new CreateConversationParticipantCommand(
                    Guid.NewGuid(),
                    conversation.Id,
                    request.CreatedBy
                ));
                await Bus.SendCommandAsync(new CreateConversationParticipantCommand(
                    Guid.NewGuid(),
                    conversation.Id,
                    request.RecipientId
                ));
            }
        }
    }
}
