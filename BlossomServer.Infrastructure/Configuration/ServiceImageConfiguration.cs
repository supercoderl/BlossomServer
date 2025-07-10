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
    public sealed class ServiceImageConfiguration : IEntityTypeConfiguration<ServiceImage>
    {
        public void Configure(EntityTypeBuilder<ServiceImage> builder)
        {
            builder.HasKey(si => si.Id);

            builder
                .Property(si => si.ImageName)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(si => si.ImageUrl).IsRequired();

            builder.Property(si => si.ServiceId).IsRequired();

            builder.Property(si => si.Description);

            builder.Property(si => si.CreatedAt).IsRequired();

            builder.HasOne(s => s.Service)
                .WithMany(si => si.ServiceImages)
                .HasForeignKey(s => s.ServiceId)
                .HasConstraintName("FK_ServiceImage_Service_ServiceId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
