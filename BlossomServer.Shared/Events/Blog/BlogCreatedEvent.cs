using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Blog
{
    public sealed class BlogCreatedEvent : DomainEvent
    {
        public BlogCreatedEvent(Guid blogId) : base(blogId)
        {
            
        }
    }
}
