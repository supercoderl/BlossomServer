using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Payments.UpdatePayment
{
    public sealed class UpdatePaymentCommand : CommandBase, IRequest
    {
        private static readonly UpdatePaymentCommandValidation s_validation = new();

        public Guid PaymentId { get; }
        public PaymentStatus Status { get; }

        public UpdatePaymentCommand(
            Guid paymentId,
            PaymentStatus status
        ) : base(Guid.NewGuid())
        {
            PaymentId = paymentId;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
