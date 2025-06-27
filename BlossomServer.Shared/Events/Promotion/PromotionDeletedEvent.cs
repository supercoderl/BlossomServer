using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Promotion
{
    public sealed class PromotionDeletedEvent : DomainEvent
    {
        public PromotionDeletedEvent(Guid promotionId) : base(promotionId)
        {
            
        }
    }
}
