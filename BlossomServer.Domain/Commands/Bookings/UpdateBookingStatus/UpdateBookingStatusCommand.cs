using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.UpdateBookingStatus
{
    public sealed class UpdateBookingStatusCommand : CommandBase, IRequest
    {
        private static readonly UpdateBookingStatusCommandValidation s_validation = new();

        public Guid BookingId { get; }
        public BookingStatus Status { get; }

        public UpdateBookingStatusCommand(
            Guid bookingId,
            BookingStatus status
        ) : base(Guid.NewGuid())
        {
            BookingId = bookingId;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
