using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Payment
{
    public sealed class PaymentCreatedEvent : DomainEvent
    {
        public PaymentCreatedEvent(Guid paymentId) : base(paymentId)
        {
            
        }
    }
}
