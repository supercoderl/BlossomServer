using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Messages.DeleteMessage
{
    public sealed class DeleteMessageCommandHandler : CommandHandlerBase, IRequestHandler<DeleteMessageCommand>
    {
        private readonly IMessageRepository _messageRepository;

        public DeleteMessageCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IMessageRepository messageRepository
        ) : base(bus, unitOfWork, notifications ) 
        {
            _messageRepository = messageRepository;
        }

        public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            if(request.MessageId.HasValue)
            {
                var message = await _messageRepository.GetByIdAsync(request.MessageId.Value);

                if(message == null)
                {
                    await NotifyAsync(new DomainNotification(
                        request.MessageType,
                        $"There is no any message with id {request.MessageId}.",
                        ErrorCodes.ObjectNotFound
                    ));

                    return;
                }

                _messageRepository.Remove(message);
            }

            if (request.ConversationId.HasValue)
            {
                var messages = await _messageRepository.GetByConversation(request.ConversationId.Value);
                if (messages.Any())
                {
                    _messageRepository.RemoveRange(messages, true);
                }
            }

            await CommitAsync();
        }
    }
}
