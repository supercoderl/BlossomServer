using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.UpdateBookingStatus
{
    public sealed class UpdateBookingStatusCommandValidation : AbstractValidator<UpdateBookingStatusCommand>
    {
        public UpdateBookingStatusCommandValidation()
        {
            RuleForId();
        }

        public void RuleForId()
        {
            RuleFor(cmd => cmd.BookingId).NotEmpty().WithErrorCode(DomainErrorCodes.Booking.EmptyId).WithMessage("Id may not be empty.");
        }
    }
}
