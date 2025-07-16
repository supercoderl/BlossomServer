using BlossomServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ConversationId).IsRequired();

            builder.Property(x => x.UserId).IsRequired();

            builder.HasOne(c => c.Conversation)
                .WithMany(x => x.ConversationParticipants)
                .HasForeignKey(c => c.ConversationId)
                .HasConstraintName("FK_ConversationParticipant_Conversation_ConversationId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.User)
                .WithMany(x => x.ConversationParticipants)
                .HasForeignKey(u => u.UserId)
                .HasConstraintName("FK_ConversationParticipant_User_UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
