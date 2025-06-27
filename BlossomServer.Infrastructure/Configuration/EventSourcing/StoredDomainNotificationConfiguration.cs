using BlossomServer.Domain.DomainNotifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Configuration.EventSourcing
{
    public sealed class StoredDomainNotificationConfiguration : IEntityTypeConfiguration<StoredDomainNotification>
    {
        public void Configure(EntityTypeBuilder<StoredDomainNotification> builder)
        {
            builder.Property(c => c.MessageType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Key)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Value)
                .HasMaxLength(1024);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.SerializedData)
                .IsRequired();

            builder.Property(c => c.User)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CorrelationId)
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Ignore(c => c.Data);

            builder.Property(c => c.SerializedData)
                .HasColumnName("Data");
        }
    }
}
