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
    public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.UserId).IsRequired();

            builder.Property(n => n.Title).IsRequired();

            builder.Property(n => n.Message).IsRequired();

            builder.Property(n => n.IsRead).IsRequired();

            builder.Property(n => n.NotificationType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(n => n.Priority).IsRequired();

            builder.Property(n => n.ExpiresAt);

            builder.Property(n => n.ActionUrl);

            builder.Property(n => n.RelatedEntityId);

            builder.Property(n => n.CreatedAt).IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(n => n.Notifications)
                .HasForeignKey(u => u.UserId)
                .HasConstraintName("FK_Notification_User_UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
