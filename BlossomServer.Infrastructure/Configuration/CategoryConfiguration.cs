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
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .Property(c => c.Name)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(c => c.IsActive).IsRequired();

            builder.Property(c => c.Icon).IsRequired();

            builder.Property(c => c.Url).IsRequired();

            builder.Property(c => c.Priority).IsRequired();

            builder.Property(c => c.CreatedAt).IsRequired();

            builder.Property(c => c.UpdatedAt);
        }
    }
}
