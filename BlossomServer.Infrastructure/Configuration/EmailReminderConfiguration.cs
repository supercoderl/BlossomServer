using BlossomServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class EmailReminderConfiguration : IEntityTypeConfiguration<EmailReminder>
    {
        public void Configure(EntityTypeBuilder<EmailReminder> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EntityId)
                .IsRequired();

            builder.Property(e => e.RecipientEmail)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.RecipientName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.RecipientType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.ReminderType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.Subject)
                .IsRequired();

            builder.Property(e => e.Message)
                .IsRequired();

            builder.Property(e => e.TargetDate)
                .IsRequired();

            builder.Property(e => e.ReminderDate)
                .IsRequired();

            builder.Property(e => e.IsSent)
                .IsRequired();

            builder.Property(e => e.IsScheduled)
                .IsRequired();

            builder.Property(e => e.HangfireJobId)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired();
        }
    }
}
