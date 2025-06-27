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
    public sealed class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
    {
        public void Configure(EntityTypeBuilder<BookingDetail> builder)
        {
            builder.HasKey(bd => bd.Id);

            builder.Property(bd => bd.BookingId).IsRequired();

            builder.Property(bd => bd.ServiceId).IsRequired();

            builder.Property(bd => bd.Quantity).IsRequired();

            builder.Property(bd => bd.UnitPrice).HasPrecision(10, 2).IsRequired();

            builder.HasOne(b => b.Booking)
                .WithMany(bd => bd.BookingDetails)
                .HasForeignKey(b => b.BookingId)
                .HasConstraintName("FK_BookingDetail_Booking_BookingId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Service)
                .WithMany(bd => bd.BookingDetails)
                .HasForeignKey(s => s.ServiceId)
                .HasConstraintName("FK_BookingDetail_Service_ServiceId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
