using BlossomServer.Domain.Entities;
using BlossomServer.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Database
{
    public partial class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<BookingDetail> BookingDetails { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Promotion> Promotions { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Technician> Technicians { get; set; } = null!;
        public DbSet<WorkSchedule> WorkSchedules { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<ServiceOption> ServiceOptions { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Conversation> Conversations { get; set; } = null!;
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; } = null!;
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } = null!;
        public DbSet<Subscriber> Subscribers { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;   
        public DbSet<ContactResponse> ContactResponses { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<Domain.Entities.FileInfo> FileInfos { get; set; } = null!;
        public DbSet<EmailReminder> EmailReminders { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.ClrType.GetProperty(DbContextUtility.IsDeleteProperty) is not null)
                {
                    modelBuilder.Entity(entity.ClrType).HasQueryFilter(DbContextUtility.GetIsDeletedRestriction(entity.ClrType));
                }
            }
            base.OnModelCreating(modelBuilder);

            ApplyConfigurations(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Enable triggers
            optionsBuilder.EnableServiceProviderCaching(false);
            optionsBuilder.EnableSensitiveDataLogging(false);
        }

        private static void ApplyConfigurations(ModelBuilder builder)
        {
            // Apply configurations for each entity type
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ServiceConfiguration());
            builder.ApplyConfiguration(new BookingDetailConfiguration());
            builder.ApplyConfiguration(new BookingConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new PromotionConfiguration());
            builder.ApplyConfiguration(new ReviewConfiguration());
            builder.ApplyConfiguration(new ServiceImageConfiguration());
            builder.ApplyConfiguration(new TechnicianConfiguration());
            builder.ApplyConfiguration(new WorkScheduleConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.ApplyConfiguration(new ServiceOptionConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new ConversationConfiguration());
            builder.ApplyConfiguration(new ConversationParticipantConfiguration());
            builder.ApplyConfiguration(new PasswordResetTokenConfiguration());
            builder.ApplyConfiguration(new SubscriberConfiguration());
            builder.ApplyConfiguration(new ContactConfiguration());
            builder.ApplyConfiguration(new ContactResponseConfiguration());
            builder.ApplyConfiguration(new AuditLogConfiguration());
            builder.ApplyConfiguration(new BlogConfiguration());
            builder.ApplyConfiguration(new FileInfoConfiguration());
            builder.ApplyConfiguration(new EmailReminderConfiguration());
        }
    }
}
