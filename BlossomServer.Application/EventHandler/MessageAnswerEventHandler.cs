using BlossomServer.Application.Interfaces;
using BlossomServer.Shared.Events.Message;
using MediatR;

namespace BlossomServer.Application.EventHandler
{
    public sealed class MessageAnswerEventHandler :
                   INotificationHandler<MessageAnswerEvent>
    {
        private readonly ISignalRService _signalRService;

        public MessageAnswerEventHandler(ISignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task Handle(MessageAnswerEvent message, CancellationToken cancellationToken)
        {
            await _signalRService.SendData("system", new
            {
                MessageId = message.AggregateId,
                Message = message.Answer
            }, "group", message.ConversationId.ToString());
        }
    }
}
