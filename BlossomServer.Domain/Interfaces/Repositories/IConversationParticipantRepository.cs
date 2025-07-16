using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IConversationParticipantRepository : IRepository<ConversationParticipant, Guid>
    {
        Task<Guid> GetConversationParticipant(Guid senderId, Guid recipientId);
    }
}
