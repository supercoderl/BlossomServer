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
    public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CustomerId);

            builder.Property(b => b.TechnicianId);

            builder.Property(b => b.ScheduleTime).IsRequired();

            builder.Property(b => b.TotalPrice).HasPrecision(10, 2).IsRequired();

            builder.Property(b => b.Status).IsRequired().HasConversion<string>();

            builder.Property(b => b.Note);

            builder.Property(b => b.GuestName);

            builder.Property(b => b.GuestPhone);

            builder.Property(b => b.GuestEmail);

            builder.Property(b => b.CreatedAt).IsRequired();

            builder.Property(b => b.UpdatedAt);

            builder.HasOne(t => t.Technician)
                .WithMany(b => b.Bookings)
                .HasForeignKey(t => t.TechnicianId)
                .HasConstraintName("FK_Booking_Techinician_TechnicianId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
