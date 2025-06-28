using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.UpdateBooking
{
    public sealed class UpdateBookingCommand : CommandBase, IRequest
    {
        private static readonly UpdateBookingCommandValidation s_validation = new();

        public Guid BookingId { get; }
        public Guid? CustomerId { get; }
        public Guid? TechnicianId { get; }
        public DateTime ScheduleTime { get; }
        public decimal TotalPrice { get; }
        public string? Note { get; }
        public string? GuestName { get; }
        public string? GuestPhone { get; }
        public string? GuestEmail { get; }
        public BookingStatus Status { get; }

        public UpdateBookingCommand(
            Guid bookingId,
            Guid? customerId,
            Guid? technicianId,
            DateTime scheduleTime,
            decimal totalPrice,
            string? note,
            string? guestName,
            string? guestPhone,
            string? guestEmail,
            BookingStatus status
        ) : base(Guid.NewGuid())
        {
            BookingId = bookingId;
            CustomerId = customerId;
            TechnicianId = technicianId;
            ScheduleTime = scheduleTime;
            TotalPrice = totalPrice;
            Note = note;
            GuestName = guestName;
            GuestPhone = guestPhone;
            GuestEmail = guestEmail;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
