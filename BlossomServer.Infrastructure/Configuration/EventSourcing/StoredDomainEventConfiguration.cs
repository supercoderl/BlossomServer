using BlossomServer.Domain.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Configuration.EventSourcing
{
    public sealed class StoredDomainEventConfiguration : IEntityTypeConfiguration<StoredDomainEvent>
    {
        public void Configure(EntityTypeBuilder<StoredDomainEvent> builder)
        {
            builder.Property(c => c.Timestamp)
                .HasColumnName("CreationDate");

            builder.Property(c => c.MessageType)
                .HasColumnName("Action")
                .HasColumnType("varchar(100)");

            builder.Property(c => c.CorrelationId)
                .HasMaxLength(100);

            builder.Property(c => c.User)
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");
        }
    }
}
