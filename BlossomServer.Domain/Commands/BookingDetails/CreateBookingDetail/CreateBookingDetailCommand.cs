using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.BookingDetails.CreateBookingDetail
{
    public sealed class CreateBookingDetailCommand : CommandBase, IRequest
    {
        private static readonly CreateBookingDetailCommandValidation s_validation = new();

        public Guid BookingDetailId { get; }
        public Guid BookingId { get; }
        public Guid? ServiceId { get; }
        public Guid? ServiceOptionId { get; }
        public int Quantity { get; }
        public decimal UnitPrice { get; }

        public CreateBookingDetailCommand(
            Guid bookingDetailId,
            Guid bookingId,
            Guid? serviceId,
            Guid? serviceOptionId,
            int quantity,
            decimal unitPrice
        ) : base(Guid.NewGuid())
        {
            BookingDetailId = bookingDetailId;
            BookingId = bookingId;
            ServiceId = serviceId;
            ServiceOptionId = serviceOptionId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
