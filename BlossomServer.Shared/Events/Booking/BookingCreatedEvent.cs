using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Booking
{
    public sealed class BookingCreatedEvent : DomainEvent 
    {
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime ScheduleTime { get; set; }

        public BookingCreatedEvent(
            Guid bookingId, 
            string customerEmail,
            string customerName,
            string customerPhone,
            DateTime scheduleTime) : base(bookingId)
        {
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            ScheduleTime = scheduleTime;
        }
    }
}
