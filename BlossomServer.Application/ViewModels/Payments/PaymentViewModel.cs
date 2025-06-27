using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Payments
{
    public sealed class PaymentViewModel
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public static PaymentViewModel FromPayment(Payment payment)
        {
            return new PaymentViewModel
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                TransactionCode = payment.TransactionCode,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}
