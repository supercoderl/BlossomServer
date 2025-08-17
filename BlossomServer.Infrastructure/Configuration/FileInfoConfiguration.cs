using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FileInfo = BlossomServer.Domain.Entities.FileInfo;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class FileInfoConfiguration : IEntityTypeConfiguration<FileInfo>
    {
        public void Configure(EntityTypeBuilder<FileInfo> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.FileId)
                .IsRequired();

            builder.Property(f => f.Url)
                .IsRequired();

            builder.Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(f => f.CreatedAt)
                .IsRequired();
        }
    }
}
