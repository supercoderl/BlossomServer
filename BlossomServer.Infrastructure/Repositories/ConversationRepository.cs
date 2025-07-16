using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class ConversationRepository : BaseRepository<Conversation, Guid>, IConversationRepository
    {
        public ConversationRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
