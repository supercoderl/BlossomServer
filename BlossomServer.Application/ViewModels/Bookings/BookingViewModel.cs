using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Bookings
{
    public sealed class BookingDetail
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public ServiceOrOption? Service { get; set; }
    }

    public sealed class ServiceOrOption
    {
        public Guid Id { get; set; }
        public int DurationMinutes { get; set; }
    }

    public sealed class BookingViewModel
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? TechnicianId { get; set; }
        public DateTime ScheduleTime { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public string? Note { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public string? GuestEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public IEnumerable<BookingDetail> BookingDetails { get; set; } = Enumerable.Empty<BookingDetail>();

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
                GuestName = booking.GuestName,
                GuestPhone = booking.GuestPhone,
                GuestEmail = booking.GuestEmail,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt,
                BookingDetails = booking.BookingDetails.Select(bd => new BookingDetail
                {
                    Id = bd.Id,
                    Quantity = bd.Quantity,
                    UnitPrice = bd.UnitPrice,
                    Service = bd.Service != null ? new ServiceOrOption
                    {
                        Id = bd.Service.Id,
                        DurationMinutes = bd.Service.DurationMinutes ?? 0
                    } : bd.ServiceOption != null ? new ServiceOrOption
                    {
                        Id = bd.ServiceOption.Id,
                        DurationMinutes = bd.ServiceOption.DurationMinutes ?? 0
                    } : null
                })
            };
        }
    }
}
