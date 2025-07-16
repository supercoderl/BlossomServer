using BlossomServer.Domain.Entities;
using BlossomServer.Shared.Events.ServiceImage;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.EventHandler
{
    public sealed class ServiceImageDomainEventHandler :
        INotificationHandler<ServiceImageCreatedEvent>,
        INotificationHandler<ServiceImageUploadProgressEvent>
    {
        private readonly IDistributedCache _distributedCache;

        public ServiceImageDomainEventHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task Handle(ServiceImageCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Pure domain logic - cache invalidation
            await _distributedCache.RemoveAsync(
                CacheKeyGenerator.GetEntityCacheKey<ServiceImage>(notification.AggregateId),
                cancellationToken);
        }

        public Task Handle(ServiceImageUploadProgressEvent notification, CancellationToken cancellationToken)
        {
            // Domain doesn't need to handle progress events
            return Task.CompletedTask;
        }
    }
}
