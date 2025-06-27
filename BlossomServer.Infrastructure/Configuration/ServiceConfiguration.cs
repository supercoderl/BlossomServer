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
    public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name).IsRequired();

            builder.Property(s => s.Description);

            builder.Property(s => s.CategoryId).IsRequired();

            builder.Property(s => s.Price).HasPrecision(10, 2).IsRequired();

            builder.Property(s => s.DurationMinutes).IsRequired();

            builder.Property(s => s.RepresentativeImage).IsRequired();

            builder.Property(s => s.CreatedAt).IsRequired();

            builder.Property(s => s.UpdatedAt);

            builder.HasOne(c => c.Category)
                .WithMany(s => s.Services)
                .HasForeignKey(c => c.CategoryId)
                .HasConstraintName("FK_Service_Category_CategoryId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
