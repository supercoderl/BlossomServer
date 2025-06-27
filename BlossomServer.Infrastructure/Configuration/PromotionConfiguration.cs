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
    public sealed class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Code).IsRequired();

            builder.Property(p => p.Description);

            builder.Property(p => p.DiscountType).HasConversion<string>().IsRequired();

            builder.Property(p => p.DiscountValue).HasPrecision(10, 2).IsRequired();

            builder.Property(p => p.MinimumSpend).HasPrecision(10, 2).IsRequired();

            builder.Property(p => p.StartDate).IsRequired();

            builder.Property(p => p.EndDate).IsRequired();

            builder.Property(p => p.MaxUsage).IsRequired();

            builder.Property(p => p.CurrentUsage).IsRequired();

            builder.Property(p => p.IsActive).IsRequired();
        }
    }
}
