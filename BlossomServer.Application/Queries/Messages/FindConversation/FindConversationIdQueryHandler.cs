using BlossomServer.Application.Queries.Messages.CheckBot;
using BlossomServer.Application.Queries.Promotions.GetById;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Domain.Commands.Conversations.CreateConversation;
using System.Reflection.Metadata;

namespace BlossomServer.Application.Queries.Messages.FindConversation
{
    public sealed class FindConversationIdQueryHandler :
                IRequestHandler<FindConversationIdQuery, Guid>
    {
        private readonly IConversationParticipantRepository _conversationParticipantRepository;
        private readonly IMediatorHandler _bus;

        public FindConversationIdQueryHandler(
            IConversationParticipantRepository conversationParticipantRepository,
            IMediatorHandler bus
        )
        {
            _conversationParticipantRepository = conversationParticipantRepository;
            _bus = bus;
        }

        public async Task<Guid> Handle(FindConversationIdQuery request, CancellationToken cancellationToken)
        {
            var id = await _conversationParticipantRepository.GetConversationParticipant(request.SenderId, request.RecipientId);

            if (id == Guid.Empty)
            {
                var newId = Guid.NewGuid();
                var isBot = request.SenderId == Domain.Constants.Ids.Seed.BotId;
                await _bus.SendCommandAsync(new CreateConversationCommand(
                    newId,
                    $"Conversation-{request.SenderId.ToString().Substring(0, 6)}-{request.RecipientId.ToString().Substring(0, 6)}",
                    Domain.Enums.ConversationType.Direct,
                    isBot ? request.RecipientId : request.SenderId,
                    isBot ? request.SenderId : request.RecipientId
                ));
                return newId;
            }

            return id;
        }
    }
}
