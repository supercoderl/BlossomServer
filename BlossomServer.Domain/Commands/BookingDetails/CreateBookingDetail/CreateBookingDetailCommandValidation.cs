using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.BookingDetails.CreateBookingDetail
{
    public sealed class CreateBookingDetailCommandValidation : AbstractValidator<CreateBookingDetailCommand>
    {
        public CreateBookingDetailCommandValidation()
        {
            
        }
    }
}
