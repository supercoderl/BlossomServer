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
    public sealed class ContactResponseConfiguration : IEntityTypeConfiguration<ContactResponse>
    {
        public void Configure(EntityTypeBuilder<ContactResponse> builder)
        {
            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.ContactId)
                .IsRequired();

            builder.Property(cr => cr.ResponseText)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(cr => cr.ResponderId)
                .IsRequired();

            builder.Property(cr => cr.CreatedAt)
                .IsRequired();

            builder.HasIndex(cr => cr.ContactId)
                .IsUnique(true);

            builder.HasOne(c => c.Contact)
                .WithOne(cr => cr.ContactResponse)
                .HasForeignKey<ContactResponse>(c => c.ContactId)
                .HasConstraintName("FK_ContactResponse_Contact_ContactId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.User)
                .WithMany(cr => cr.ContactResponses)
                .HasForeignKey(u => u.ResponderId)
                .HasConstraintName("FK_ContactResponse_User_ResponderId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
