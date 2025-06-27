using BlossomServer.Domain.Commands;
using BlossomServer.Shared.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces
{
    public interface IMediatorHandler
    {
        Task RaiseEventAsync<T>(T @event) where T : DomainEvent;
        Task SendCommandAsync<T>(T command) where T : CommandBase;
        Task<TResponse> QueryAsync<TResponse>(IRequest<TResponse> query);
    }
}
