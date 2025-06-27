using BlossomServer.Domain.Commands;
using BlossomServer.Domain.DomainEvents;
using BlossomServer.Domain.EventHandler.Fanout;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Shared.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IDomainEventStore _domainEventStore;
        private readonly IFanoutEventHandler _fanoutEventHandler;

        public InMemoryBus(
            IMediator mediator,
            IDomainEventStore domainEventStore,
            IFanoutEventHandler fanoutEventHandler
        )
        {
            _mediator = mediator;
            _domainEventStore = domainEventStore;
            _fanoutEventHandler = fanoutEventHandler;
        }

        public Task<TResponse> QueryAsync<TResponse>(IRequest<TResponse> query)
        {
            return _mediator.Send(query);
        }

        public async Task RaiseEventAsync<T>(T @event) where T : DomainEvent
        {
            await _domainEventStore.SaveAsync(@event);

            await _mediator.Publish(@event);

            await _fanoutEventHandler.HandleDomainEventAsync(@event);
        }

        public Task SendCommandAsync<T>(T command) where T : CommandBase
        {
            return _mediator.Send(command); 
        }
    }
}
