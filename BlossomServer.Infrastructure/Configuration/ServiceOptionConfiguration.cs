using BlossomServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class ServiceOptionConfiguration : IEntityTypeConfiguration<ServiceOption>
    {
        public void Configure(EntityTypeBuilder<ServiceOption> builder)
        {
            builder.HasKey(so => so.Id);

            builder.Property(so => so.VariantName).IsRequired();

            builder.Property(so => so.PriceFrom).IsRequired().HasPrecision(10, 2);

            builder.Property(so => so.PriceTo).HasPrecision(10, 2);

            builder.Property(so => so.DurationMinutes);

            builder.HasOne(s => s.Service)
                .WithMany(so => so.ServiceOptions)
                .HasForeignKey(s => s.ServiceId)
                .HasConstraintName("FK_ServiceOption_Service_ServiceId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
