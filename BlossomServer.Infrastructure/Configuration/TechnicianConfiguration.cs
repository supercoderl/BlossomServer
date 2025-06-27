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
    public sealed class TechnicianConfiguration : IEntityTypeConfiguration<Technician>
    {
        public void Configure(EntityTypeBuilder<Technician> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId).IsRequired();

            builder.Property(t => t.Bio).IsRequired();

            builder.Property(t => t.Rating).IsRequired();

            builder.Property(t => t.YearsOfExperience).IsRequired();

            builder.HasOne(u => u.User)
                .WithOne(t => t.Technician)
                .HasForeignKey<Technician>(u => u.UserId)
                .HasConstraintName("FK_Techinician_User_UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
