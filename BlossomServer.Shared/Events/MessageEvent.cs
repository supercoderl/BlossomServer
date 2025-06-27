using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events
{
    public abstract class MessageEvent : IRequest
    {
        public Guid AggregateId { get; private set; }
        public string MessageType { get; protected set; }

        protected MessageEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            MessageType = GetType().Name;
        }

        protected MessageEvent(Guid aggregateId, string? messageType)
        {
            AggregateId = aggregateId;
            MessageType = messageType ?? string.Empty;
        }
    }
}
