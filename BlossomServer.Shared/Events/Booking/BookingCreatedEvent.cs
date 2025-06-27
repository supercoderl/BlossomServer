using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Booking
{
    public sealed class BookingCreatedEvent : DomainEvent 
    {
        public BookingCreatedEvent(Guid bookingId) : base(bookingId)
        {
            
        }
    }
}
