using BlossomServer.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.EventHandler.Fanout
{
    public interface IFanoutEventHandler
    {
        Task<T> HandleDomainEventAsync<T>(T @event) where T : DomainEvent;
    }
}
