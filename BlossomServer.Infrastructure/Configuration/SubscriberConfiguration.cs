using BlossomServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(s => s.Email)
                .IsUnique();

            builder.Property(s => s.CreatedAt)
                .IsRequired();
        }
    }
}
