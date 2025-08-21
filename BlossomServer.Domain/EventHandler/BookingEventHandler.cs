using BlossomServer.Domain.Entities;
using BlossomServer.Shared.Events.Admin;
using BlossomServer.Shared.Events.Booking;
using BlossomServer.Shared.Events.User;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace BlossomServer.Domain.EventHandler
{
    public sealed class BookingEventHandler :
        INotificationHandler<BookingCreatedEvent>,
        INotificationHandler<NotificationRequiredEvent>
    {
        private readonly IDistributedCache _distributedCache;

        public BookingEventHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _distributedCache.RemoveAsync(
                CacheKeyGenerator.GetEntityCacheKey<Booking>(notification.AggregateId),
                cancellationToken);
        }

        public Task Handle(NotificationRequiredEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
