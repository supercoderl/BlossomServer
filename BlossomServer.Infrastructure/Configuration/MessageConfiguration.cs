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
    public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.SenderId).IsRequired();

            builder.Property(m => m.RecipientId);

            builder.Property(m => m.ConversationId).IsRequired();

            builder.Property(m => m.MessageText).IsRequired();

            builder.Property(m => m.MessageType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(m => m.IsRead).IsRequired();

            builder.Property(m => m.UnreadCount).IsRequired();

            builder.HasOne(u => u.Sender)
                .WithMany(m => m.SenderMessages)
                .HasForeignKey(u => u.SenderId)
                .HasConstraintName("FK_Message_User_SenderId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Recipient)
                .WithMany(m => m.RecipientMessages)
                .HasForeignKey(u => u.RecipientId)
                .HasConstraintName("FK_Message_User_RecipientId")
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.Conversation)
                .WithMany(m => m.Messages)
                .HasForeignKey(c => c.ConversationId)
                .HasConstraintName("FK_Message_Conversation_ConversationId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
