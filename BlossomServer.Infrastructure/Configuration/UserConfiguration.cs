using BlossomServer.Domain.Constants;
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
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.ToTable("Users", tb => tb.HasTrigger("TR_Users_Audit"));

            builder.HasData(new User(
                Ids.Seed.UserId,
                BCrypt.Net.BCrypt.HashPassword("123456789aA@"),
                "Admin",
                "Super",
                "admin@gmail.com",
                "+1586324954",
                "https://haamc.offerslokam.com/wp-content/uploads/2023/09/client-dummy-Google-Search-1.png",
                null,
                Domain.Enums.Gender.Male,
                null,
                DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                Domain.Enums.UserRole.Admin,
                Domain.Enums.UserStatus.Active
            ));

            builder.HasData(new User(
                Ids.Seed.BotId,
                BCrypt.Net.BCrypt.HashPassword("123456789aA@"),
                "Bot",
                "System",
                "bot@nblossom.com",
                "+1111111111",
                "https://avatars.githubusercontent.com/u/6422482?v=4",
                null,
                Domain.Enums.Gender.Unknow,
                null,
                DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                Domain.Enums.UserRole.Bot,
                Domain.Enums.UserStatus.Active
            ));
        }
    }
}
