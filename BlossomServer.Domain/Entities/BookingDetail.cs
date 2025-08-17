using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class BookingDetail : Entity<Guid>
    {
        public Guid BookingId { get; private set; }
        public Guid? ServiceId { get; private set; }
        public Guid? ServiceOptionId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        [ForeignKey("BookingId")]
        [InverseProperty("BookingDetails")]
        public virtual Booking? Booking { get; set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("BookingDetails")]
        public virtual Service? Service { get; set; }

        [ForeignKey("ServiceOptionId")]
        [InverseProperty("BookingDetails")]
        public virtual ServiceOption? ServiceOption { get; set; }

        public BookingDetail(
            Guid id,
            Guid bookingId,
            Guid? serviceId,
            Guid? serviceOptionId,
            int quantity,
            decimal unitPrice
        ) : base(id)
        {
            BookingId = bookingId;
            ServiceId = serviceId;
            ServiceOptionId = serviceOptionId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void SetBookingId( Guid bookingId ) { BookingId = bookingId; }
        public void SetServiceId( Guid? serviceId ) { ServiceId = serviceId; }
        public void SetServiceOptionId(Guid? serviceOptionId) {  ServiceOptionId = serviceOptionId; }
        public void SetQuantity( int quantity ) { Quantity = quantity; }
        public void SetUnitPrice( decimal unitPrice ) { UnitPrice = unitPrice; }
        public void SetService(Service? service ) { Service = service; }
        public void SetServiceOption(ServiceOption? serviceOption ) { ServiceOption = serviceOption; }
    }
}
