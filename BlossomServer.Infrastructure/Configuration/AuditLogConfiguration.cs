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
    public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(e => e.TableName)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(e => e.Operation)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(e => e.PrimaryKey)
                .HasMaxLength(450);

            builder.Property(e => e.ColumnName)
                .HasMaxLength(128);

            builder.Property(e => e.ChangedBy)
                .HasMaxLength(450);

            builder.Property(e => e.ApplicationUser)
                .HasMaxLength(450);

            builder.Property(e => e.IpAddress)
                .HasMaxLength(45);

            builder.Property(e => e.SessionId)
                .HasMaxLength(450);

            builder.Property(e => e.ChangedDate)
                .IsRequired();

            builder.HasIndex(e => e.TableName);
            builder.HasIndex(e => e.PrimaryKey);
            builder.HasIndex(e => e.ChangedDate);
            builder.HasIndex(e => e.ChangedBy);
        }
    }
}
