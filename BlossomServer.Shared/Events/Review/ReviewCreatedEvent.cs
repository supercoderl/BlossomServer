using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Review
{
    public sealed class ReviewCreatedEvent : DomainEvent
    {
        public ReviewCreatedEvent(Guid reviewId) : base(reviewId)
        {
            
        }
    }
}
