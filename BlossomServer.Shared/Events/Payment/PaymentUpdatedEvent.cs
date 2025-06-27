using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Payment
{
    public sealed class PaymentUpdatedEvent : DomainEvent
    {
        public PaymentUpdatedEvent(Guid paymentId) : base(paymentId)
        {
            
        }
    }
}
