using BlossomServer.Domain.Enums;
using BlossomServer.SharedKernel.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlossomServer.Domain.Entities
{
    public class Booking : Entity<Guid>
    {
        public Guid? CustomerId { get; private set; }
        public Guid? TechnicianId { get; private set; }
        public DateTime ScheduleTime { get; private set; }
        public decimal TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public string? Note { get; private set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public string? GuestEmail { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        [InverseProperty("Booking")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

        [ForeignKey("TechnicianId")]
        [InverseProperty("Bookings")]
        public virtual Technician? Technician { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Bookings")]
        public virtual User? Customer { get; set; }

        [InverseProperty("Booking")]
        public virtual Payment? Payment { get; set; }

        [InverseProperty("Booking")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public Booking(
            Guid id,
            Guid? customerId,
            Guid? technicianId,
            DateTime scheduleTime,
            decimal totalPrice,
            BookingStatus status,
            string? note,
            string? guestName,
            string? guestPhone,
            string? guestEmail
        ) : base(id)
        {
            CustomerId = customerId;
            TechnicianId = technicianId;
            ScheduleTime = scheduleTime;
            TotalPrice = totalPrice;
            Status = status;
            Note = note;
            GuestName = guestName;
            GuestPhone = guestPhone;
            GuestEmail = guestEmail;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetCustomerId(Guid? customerId) { CustomerId = customerId; }
        public void SetTechnicianId(Guid? technicianId) { TechnicianId = technicianId; }
        public void SetScheduleTime(DateTime scheduleTime) { ScheduleTime = scheduleTime; }
        public void SetTotalPrice(decimal totalPrice) { TotalPrice = totalPrice; }
        public void SetStatus(BookingStatus status) { Status = status; }
        public void SetNote(string? note) { Note = note; }
        public void SetGuestName(string? guestName) { GuestName = guestName; }
        public void SetGuestPhone(string? guestPhone) { GuestPhone = guestPhone; }
        public void SetGuestEmail(string? guestEmail) { GuestEmail = guestEmail; }
        public void SetUpdatedAt(DateTime? updatedAt) { UpdatedAt = updatedAt.HasValue ? updatedAt.Value : TimeZoneHelper.GetLocalTimeNow(); }
        public void SetCreatedAt(DateTime createdAt) { CreatedAt = createdAt; }
        public void SetBookingDetails(ICollection<BookingDetail> bookingDetails) { BookingDetails = bookingDetails; }

        public void SetTechnician(Technician? technician) { Technician = technician; }
        public void SetCustomer(User? customer) { Customer = customer; }
    }
}
