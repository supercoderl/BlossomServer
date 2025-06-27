using BlossomServer.Domain.Enums;
using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Booking : Entity<Guid>
    {
        public Guid CustomerId { get; private set; }
        public Guid? TechnicianId { get; private set; }
        public DateTime ScheduleTime { get; private set; }
        public decimal TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public string? Note { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        [InverseProperty("Booking")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

        [ForeignKey("TechnicianId")]
        [InverseProperty("Bookings")]
        public virtual Technician? Technician { get; set; }

        [InverseProperty("Booking")]
        public virtual Payment? Payment { get; set; }

        [InverseProperty("Booking")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public Booking(
            Guid id,
            Guid customerId,
            Guid? technicianId,
            DateTime scheduleTime,
            decimal totalPrice,
            BookingStatus status,
            string? note
        ) : base(id)
        {
            CustomerId = customerId;
            TechnicianId = technicianId;
            ScheduleTime = scheduleTime;
            TotalPrice = totalPrice;
            Status = status;
            Note = note;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetCustomerId( Guid customerId ) { CustomerId = customerId; }
        public void SetTechnicianId(Guid? technicianId) { TechnicianId = technicianId; }
        public void SetScheduleTime(DateTime scheduleTime ) { ScheduleTime = scheduleTime; }
        public void SetTotalPrice(decimal totalPrice) { TotalPrice = totalPrice; }
        public void SetStatus(BookingStatus status) { Status = status; }
        public void SetNote(string? note) { Note = note; }
        public void SetUpdatedAt() { UpdatedAt = TimeZoneHelper.GetLocalTimeNow(); }
    }
}
