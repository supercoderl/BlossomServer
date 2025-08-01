﻿using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message, Guid>
    {
        public Task<IEnumerable<Message>> GetByConversation(Guid conversationId);
    }
}
