using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.BookingDetail
{
    public sealed class BookingDetailCreatedEvent : DomainEvent
    {
        public BookingDetailCreatedEvent(Guid bookingDetailId) : base(bookingDetailId)
        {
            
        }
    }
}
