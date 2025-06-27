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
    public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.BookingId).IsRequired();

            builder.Property(r => r.CustomerId).IsRequired();

            builder.Property(r => r.TechnicianId).IsRequired();

            builder.Property(r => r.Rating).IsRequired();

            builder.Property(r => r.Comment).IsRequired();  

            builder.Property(r => r.CreatedAt).IsRequired();

            builder.HasOne(b => b.Booking)
                .WithMany(r => r.Reviews)
                .HasForeignKey(b => b.BookingId)
                .HasConstraintName("FK_Review_Booking_BookingId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(u => u.CustomerId)
                .HasConstraintName("FK_Review_User_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Technician)
                .WithMany(r => r.Reviews)
                .HasForeignKey(t => t.TechnicianId)
                .HasConstraintName("FK_Review_Technician_TechnicianId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
