using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Blog
{
    public sealed class BlogUpdatedEvent : DomainEvent
    {
        public BlogUpdatedEvent(Guid blogId) : base(blogId)
        {
            
        }
    }
}
