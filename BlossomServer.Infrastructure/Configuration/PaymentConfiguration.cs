using BlossomServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.BookingId).IsRequired();

            builder.Property(p => p.Amount).HasPrecision(10, 2).IsRequired();

            builder.Property(p => p.Method).HasConversion<string>().IsRequired();

            builder.Property(p => p.Status).HasConversion<string>().IsRequired();

            builder.Property(p => p.TransactionCode).IsRequired();

            builder.Property(p => p.CreatedAt).IsRequired();

            builder.HasOne(b => b.Booking)
                .WithOne(p => p.Payment)
                .HasForeignKey<Payment>(b => b.BookingId)
                .HasConstraintName("FK_Payment_Booking_BookingId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
