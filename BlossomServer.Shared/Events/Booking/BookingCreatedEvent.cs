using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Booking
{
    public sealed class BookingCreatedEvent : DomainEvent 
    {
        public Guid? ReceiverId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime ScheduleTime { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ServiceOptionId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public BookingCreatedEvent(
            Guid? receiverId,
            Guid bookingId, 
            string customerEmail,
            string customerName,
            string customerPhone,
            DateTime scheduleTime,
            Guid? serviceId,
            Guid? serviceOptionId,
            int quantity,
            decimal price) : base(bookingId)
        {
            ReceiverId = receiverId;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            ScheduleTime = scheduleTime;
            ServiceId = serviceId;
            ServiceOptionId = serviceOptionId;
            Quantity = quantity;
            Price = price;
        }
    }
}
