using BlossomServer.SharedKernel.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlossomServer.Domain.Entities
{
    public class Review : Entity<Guid>
    {
        public Guid BookingId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid TechnicianId { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("BookingId")]
        [InverseProperty("Reviews")]
        public virtual Booking? Booking { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Reviews")]
        public virtual User? User { get; set; }

        [ForeignKey("TechnicianId")]
        [InverseProperty("Reviews")]
        public virtual Technician? Technician { get; set; }

        public Review(
            Guid id,
            Guid bookingId,
            Guid customerId,
            Guid technicianId,
            int rating,
            string comment
        ) : base(id)
        {
            BookingId = bookingId;
            CustomerId = customerId;
            TechnicianId = technicianId;
            Rating = rating;
            Comment = comment;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetBookingId(Guid bookingId) { BookingId = bookingId; }
        public void SetCustomerId(Guid customerId) { CustomerId = customerId; }
        public void SetTechnicianId(Guid technicianId) { TechnicianId = technicianId; }
        public void SetRating(int rating) { Rating = rating; }
        public void SetComment(string comment) { Comment = comment; }
    }
}
