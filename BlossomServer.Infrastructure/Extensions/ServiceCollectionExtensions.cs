using BlossomServer.Application.Interfaces;
using BlossomServer.Domain.DomainEvents;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.BackgroundServices;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Infrastructure.BackgroundServices;
using BlossomServer.Infrastructure.Database;
using BlossomServer.Infrastructure.EventSourcing;
using BlossomServer.Infrastructure.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlossomServer.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            string migrationAssemblyName,
            string connectionString = "DefaultConnection"
        )
        {
            // Add event store db context
            services.AddDbContext<EventStoreDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(migrationAssemblyName)
                )
            );

            // Add domain notification store db context
            services.AddDbContext<DomainNotificationStoreDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(migrationAssemblyName)
                )
            );

            //Core
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<IDomainEventStore, EventStore>();
            services.AddScoped<IEventStoreContext, EventStoreContext>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceImageRepository, ServiceImageRepository>();
            services.AddScoped<ITechnicianRepository, TechnicianRepository>();
            services.AddScoped<IWorkScheduleRepository, WorkScheduleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IServiceOptionRepository, ServiceOptionRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IConversationParticipantRepository, ConversationParticipantRepository>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<ISubscriberRepository, SubscriberRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactResponseRepository, ContactResponseRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IContextInfoService, AuditLogRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IFileInfoRepository, FileInfoRepository>();
            services.AddScoped<IEmailReminderRepository, EmailReminderRepository>();

            return services;
        }

        public static IServiceCollection AddBackground(this IServiceCollection services)
        {
            services.AddHostedService<DatabaseWarmupService>();

            return services;
        }

        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(options => options
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();

            services.AddScoped<IEmailReminderBackgroundService, EmailReminderBackgroundService>();

            return services;
        }
    }
}
