using BlossomServer.Domain.DomainEvents;
using BlossomServer.Domain.DomainNotifications;
using BlossomServer.Domain.Notifications;
using BlossomServer.Infrastructure.Database;
using BlossomServer.Shared.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.EventSourcing
{
    public sealed class EventStore : IDomainEventStore
    {
        private readonly EventStoreDbContext _eventStoreDbContext;
        private readonly DomainNotificationStoreDbContext _domainNotificationStoreDbContext;
        private readonly IEventStoreContext _context;

        public EventStore(
            EventStoreDbContext eventStoreDbContext,
            DomainNotificationStoreDbContext domainNotificationStoreDbContext,
            IEventStoreContext context
        )
        {
            _eventStoreDbContext = eventStoreDbContext;
            _domainNotificationStoreDbContext = domainNotificationStoreDbContext;
            _context = context;
        }

        public async Task SaveAsync<T>(T domainEvent) where T : DomainEvent
        {
            var serializedData = JsonConvert.SerializeObject(domainEvent);

            switch(domainEvent)
            {
                case DomainNotification d:
                    var storedDomainNotification = new StoredDomainNotification(
                        d,
                        serializedData,
                        _context.GetUserEmail(),
                        _context.GetCorrelationId()
                    );

                    _domainNotificationStoreDbContext.StoredDomainNotifications.Add(storedDomainNotification);
                    await _domainNotificationStoreDbContext.SaveChangesAsync();

                    break;
                default:
                    var storedDomainEvent = new StoredDomainEvent(
                        domainEvent,
                        serializedData,
                        _context.GetUserEmail(),
                        _context.GetCorrelationId()
                    );

                    _eventStoreDbContext.StoredDomainEvents.Add(storedDomainEvent);
                    await _eventStoreDbContext.SaveChangesAsync();

                    break;
            }
        }
    }
}
