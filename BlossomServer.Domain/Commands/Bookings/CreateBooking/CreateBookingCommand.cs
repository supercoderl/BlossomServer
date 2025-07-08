using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.CreateBooking
{
    public sealed class CreateBookingCommand : CommandBase, IRequest
    {
        private static readonly CreateBookingCommandValidation s_validation = new();

        public Guid BookingId { get; }
        public Guid? CustomerId { get; }
        public Guid? TechnicianId { get; }
        public string ScheduleTime { get; }
        public Guid? ServiceId { get; }
        public Guid? ServiceOptionId { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public string? Note { get; }
        public string? GuestName { get; }
        public string? GuestPhone { get; }
        public string? GuestEmail { get; }

        public CreateBookingCommand(
            Guid bookingId,
            Guid? customerId,
            Guid? technicianId,
            string scheduleTime,
            Guid? serviceId,
            Guid? serviceOptionId,
            int quantity,
            decimal price,
            string? note,
            string? guestName,
            string? guestPhone,
            string? guestEmail
        ) : base(Guid.NewGuid())
        {
            BookingId = bookingId;
            CustomerId = customerId;
            TechnicianId = technicianId;
            ScheduleTime = scheduleTime;
            ServiceId = serviceId;
            ServiceOptionId = serviceOptionId;
            Quantity = quantity;
            Price = price;
            Note = note;
            GuestName = guestName;
            GuestPhone = guestPhone;
            GuestEmail = guestEmail;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
