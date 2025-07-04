﻿using BlossomServer.Domain.Constants;
using BlossomServer.Domain.Entities;
using BlossomServer.SharedKernel.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlossomServer.Infrastructure.Configuration
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Password).IsRequired();

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.HasData(new User(
                Ids.Seed.UserId,
                BCrypt.Net.BCrypt.HashPassword("123456"),
                "Admin",
                "Super",
                "admin@gmail.com",
                "+1586324954",
                "https://haamc.offerslokam.com/wp-content/uploads/2023/09/client-dummy-Google-Search-1.png",
                Domain.Enums.Gender.Male,
                DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                Domain.Enums.UserRole.Admin,
                Domain.Enums.UserStatus.Active
            ));
        }
    }
}
