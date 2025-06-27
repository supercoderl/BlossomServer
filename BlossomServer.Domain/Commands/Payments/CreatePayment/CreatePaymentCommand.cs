using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Payments.CreatePayment
{
    public sealed class CreatePaymentCommand : CommandBase, IRequest
    {
        private static readonly CreatePaymentCommandValidation s_validation = new();

        public Guid PaymentId { get; }
        public Guid BookingId { get; }
        public decimal Amount { get; }
        public PaymentMethod Method { get; }
        public string TransactionCode { get; }

        public CreatePaymentCommand(
            Guid paymentId,
            Guid bookingId,
            decimal amount,
            PaymentMethod method,
            string transactionCode
        ) : base(Guid.NewGuid())
        {
            PaymentId = paymentId;
            BookingId = bookingId;
            Amount = amount;
            Method = method;
            TransactionCode = transactionCode;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
