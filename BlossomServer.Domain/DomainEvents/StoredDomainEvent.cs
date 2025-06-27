using BlossomServer.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.DomainEvents
{
    public class StoredDomainEvent : DomainEvent
    {
        public Guid Id { get; private set; }
        public string Data { get; private set; }
        public string User { get; private set; }
        public string CorrelationId { get; private set; }

        public StoredDomainEvent(
            DomainEvent domainEvent,
            string data,
            string user,
            string correlationId
        ) : base(domainEvent.AggregateId, domainEvent.MessageType)
        {
            Id = Guid.NewGuid();
            Data = data;
            User = user;
            CorrelationId = correlationId;
        }

        // EF Core constructor
        protected StoredDomainEvent() : base(Guid.NewGuid())
        {
        }
    }
}
