using BlossomServer.Domain.DomainEvents;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Infrastructure.Database;
using BlossomServer.Infrastructure.EventSourcing;
using BlossomServer.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

            return services;
        }
    }
}
