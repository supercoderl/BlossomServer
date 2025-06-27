using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.UpdateBooking
{
    public sealed class UpdateBookingCommandValidation : AbstractValidator<UpdateBookingCommand>
    {
        public UpdateBookingCommandValidation()
        {
            
        }
    }
}
