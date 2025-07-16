using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class MessageRepository : BaseRepository<Message, Guid>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Message>> GetByConversation(Guid conversationId)
        {
            return await DbSet.Where(x => x.ConversationId == conversationId).ToListAsync();
        }
    }
}
