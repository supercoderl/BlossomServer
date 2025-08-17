using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Subscriber;
using MediatR;

namespace BlossomServer.Domain.Commands.Subscribers.Subscribe
{
    public sealed class SubscribeCommandHandler : CommandHandlerBase, IRequestHandler<SubscribeCommand>
    {
        private readonly ISubscriberRepository _subscriberRepository;

        public SubscribeCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            ISubscriberRepository subscriberRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _subscriberRepository = subscriberRepository;
        }

        public async Task Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var subscriber = await _subscriberRepository.GetByEmail(request.Email, cancellationToken);

            if(subscriber != null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is already a subscriber with the email {request.Email}.",
                    ErrorCodes.ObjectAlreadyExists
                ));

                return;
            }

            subscriber = new Subscriber(
                request.SubscriberId,
                request.Email
            );

            _subscriberRepository.Add(subscriber);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new SubscribeEvent(subscriber.Id, subscriber.Email));
            }
        }
    }
}
