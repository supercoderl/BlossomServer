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
    public sealed class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(b => b.Slug)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(b => b.Content)
                .IsRequired()
                .HasColumnType("ntext");

            builder.Property(builder => builder.AuthorId)
                .IsRequired();

            builder.Property(b => b.Tags)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt);

            builder.Property(b => b.PublishedAt)
                .IsRequired();

            builder.Property(b => b.IsPublished)
                .IsRequired();

            builder.Property(b => b.ThumbnailUrl)
                .IsRequired();

            builder.Property(b => b.ViewsCount)
                .IsRequired();

            builder.Property(b => b.CommentsCount)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(b => b.Blogs)
                .HasForeignKey(u => u.AuthorId)
                .HasConstraintName("FK_Blogs_Users_AuthorId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
