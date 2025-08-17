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
    public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.UserId)
                .IsRequired();

            builder.Property(p => p.Token)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(p => p.ExpirationDate)
                .IsRequired();

            builder.Property(p => p.IsUsed)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(u => u.UserId)
                .HasConstraintName("FK_PasswordResetToken_User_UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
