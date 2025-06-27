using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Booking
{
    public sealed class BookingUpdatedEvent : DomainEvent
    {
        public BookingUpdatedEvent(Guid bookingId) : base(bookingId)
        {
            
        }
    }
}
