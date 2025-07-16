using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class ConversationParticipantRepository : BaseRepository<ConversationParticipant, Guid>, IConversationParticipantRepository
    {
        public ConversationParticipantRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Guid> GetConversationParticipant(Guid senderId, Guid recipientId)
        {
            return await DbSet
                .Where(cp => cp.UserId == senderId || cp.UserId == recipientId)
                .GroupBy(cp => cp.ConversationId)
                .Where(g => g.Count() == 2 &&
                            g.Any(p => p.UserId == senderId) &&
                            g.Any(p => p.UserId == recipientId))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();
        }
    }
}
