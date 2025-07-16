using MediatR;

namespace BlossomServer.Application.Queries.Messages.CheckBot
{
    public sealed record CheckBotQuery(Guid Id) : IRequest<bool>;
}
