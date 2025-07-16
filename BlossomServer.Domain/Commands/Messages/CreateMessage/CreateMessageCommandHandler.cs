using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Message;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Messages.CreateMessage
{
    public sealed class CreateMessageCommandHandler : CommandHandlerBase, IRequestHandler<CreateMessageCommand>
    {
        private readonly IMessageRepository _messageRepository;

        public CreateMessageCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IMessageRepository messageRepository
        ) : base(bus, unitOfWork, notifications )
        {
            _messageRepository = messageRepository;
        }

        public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var message = new Entities.Message(
                request.MessageId,
                request.SenderId,
                request.RecipientId,
                request.ConversationId,
                request.MessageText,
                request.MessageType
            );

            _messageRepository.Add( message );

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new MessageCreatedEvent(message.Id));
                if(request.SenderId == Constants.Ids.Seed.BotId)
                {
                    await Bus.RaiseEventAsync(new MessageAnswerEvent(message.Id, request.MessageText, request.ConversationId));
                }
            }
        }
    }
}
