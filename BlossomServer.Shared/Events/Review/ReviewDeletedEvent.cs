using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Review
{
    public sealed class ReviewDeletedEvent : DomainEvent
    {
        public ReviewDeletedEvent(Guid reviewId) : base(reviewId)
        {
            
        }
    }
}
