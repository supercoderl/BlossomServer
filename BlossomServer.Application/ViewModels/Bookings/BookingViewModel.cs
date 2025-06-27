using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Bookings
{
    public sealed class BookingViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? TechnicianId { get; set; }
        public DateTime ScheduleTime { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static BookingViewModel FromBooking(Booking booking)
        {
            return new BookingViewModel
            {
                Id = booking.Id,
                CustomerId = booking.CustomerId,
                TechnicianId = booking.TechnicianId,
                ScheduleTime = booking.ScheduleTime,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                Note = booking.Note,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt
            };
        }
    }
}
