using MediatR;

namespace BlossomServer.Application.Queries.Messages.FindConversation
{
    public sealed record FindConversationIdQuery(Guid SenderId, Guid RecipientId) : IRequest<Guid>;
}
