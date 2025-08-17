using BlossomServer.Domain.Enums;
using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Payment : Entity<Guid>
    {
        public Guid BookingId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string TransactionCode { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("BookingId")]
        public virtual Booking? Booking { get; set; }

        public Payment(
            Guid id,
            Guid bookingId,
            decimal amount,
            PaymentMethod method,
            string transactionCode
        ) : base(id)
        {
            BookingId = bookingId;
            Amount = amount;
            Method = method;
            Status = PaymentStatus.Pending;
            TransactionCode = transactionCode;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetBookingId( Guid bookingId ) { BookingId = bookingId; }
        public void SetAmount( decimal amount ) { Amount = amount; }
        public void SetMethod( PaymentMethod method ) { Method = method; }
        public void SetStatus( PaymentStatus status ) { Status = status; }
        public void SetTransactionCode( string transactionCode ) { TransactionCode = transactionCode; }
        public void SetCreatedAt(DateTime createdAt) { CreatedAt = createdAt; }
    }
}
