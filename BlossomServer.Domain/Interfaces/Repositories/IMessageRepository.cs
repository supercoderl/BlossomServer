using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message, Guid>
    {
        public Task<IEnumerable<Message>> GetByConversation(Guid conversationId);

        Task<IEnumerable<Message>> GetAllMessagesBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            Guid conversationId,
            CancellationToken cancellationToken = default
        );
    }
}
