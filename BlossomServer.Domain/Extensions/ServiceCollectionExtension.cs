using BlossomServer.Domain.Commands.BookingDetails.CreateBookingDetail;
using BlossomServer.Domain.Commands.Bookings.CreateBooking;
using BlossomServer.Domain.Commands.Bookings.UpdateBooking;
using BlossomServer.Domain.Commands.Categories.CreateCategory;
using BlossomServer.Domain.Commands.Categories.DeleteCategory;
using BlossomServer.Domain.Commands.Categories.UpdateCategory;
using BlossomServer.Domain.Commands.Files.UploadFile;
using BlossomServer.Domain.Commands.Notifications.CreateNotification;
using BlossomServer.Domain.Commands.Notifications.DeleteNotification;
using BlossomServer.Domain.Commands.Notifications.UpdateStatusNotification;
using BlossomServer.Domain.Commands.Payments.CreatePayment;
using BlossomServer.Domain.Commands.Payments.UpdatePayment;
using BlossomServer.Domain.Commands.Promotions.CreatePromotion;
using BlossomServer.Domain.Commands.Promotions.DeletePromotion;
using BlossomServer.Domain.Commands.Promotions.UpdatePromotion;
using BlossomServer.Domain.Commands.RefreshToken.CreateRefreshToken;
using BlossomServer.Domain.Commands.Reviews.CreateReview;
using BlossomServer.Domain.Commands.Reviews.DeleteReview;
using BlossomServer.Domain.Commands.Reviews.UpdateReview;
using BlossomServer.Domain.Commands.ServiceImages.CreateServiceImage;
using BlossomServer.Domain.Commands.ServiceImages.UpdateServiceImage;
using BlossomServer.Domain.Commands.ServiceOptions.CreateServiceOption;
using BlossomServer.Domain.Commands.ServiceOptions.DeleteServiceOption;
using BlossomServer.Domain.Commands.ServiceOptions.UpdateServiceOption;
using BlossomServer.Domain.Commands.Services.CreateService;
using BlossomServer.Domain.Commands.Services.DeleteService;
using BlossomServer.Domain.Commands.Services.UpdateService;
using BlossomServer.Domain.Commands.Technicians.CreateTechnician;
using BlossomServer.Domain.Commands.Technicians.DeleteTechnician;
using BlossomServer.Domain.Commands.Technicians.UpdateTechnician;
using BlossomServer.Domain.Commands.Users.ChangePassword;
using BlossomServer.Domain.Commands.Users.CreateUser;
using BlossomServer.Domain.Commands.Users.DeleteUser;
using BlossomServer.Domain.Commands.Users.Login;
using BlossomServer.Domain.Commands.Users.RefreshToken;
using BlossomServer.Domain.Commands.Users.UpdateUser;
using BlossomServer.Domain.Commands.WorkSchedules.CreateWorkSchedule;
using BlossomServer.Domain.Commands.WorkSchedules.DeleteWorkSchedule;
using BlossomServer.Domain.Commands.WorkSchedules.UpdateWorkSchedule;
using BlossomServer.Domain.EventHandler;
using BlossomServer.Domain.EventHandler.Fanout;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Settings;
using BlossomServer.Shared.Events.User;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlossomServer.Domain.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            // User
            services.AddScoped<IRequestHandler<CreateUserCommand>, CreateUserCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateUserCommand>, UpdateUserCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
            services.AddScoped<IRequestHandler<ChangePasswordCommand>, ChangePasswordCommandHandler>();
            services.AddScoped<IRequestHandler<LoginUserCommand, object>, LoginUserCommandHandler>();
            services.AddScoped<IRequestHandler<RefreshTokenCommand, object>, RefreshTokenCommandHandler>();

            // Booking
            services.AddScoped<IRequestHandler<CreateBookingCommand>, CreateBookingCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateBookingCommand>, UpdateBookingCommandHandler>();

            // Category
            services.AddScoped<IRequestHandler<CreateCategoryCommand>, CreateCategoryCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCategoryCommand>, UpdateCategoryCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteCategoryCommand>, DeleteCategoryCommandHandler>();

            // Notification
            services.AddScoped<IRequestHandler<CreateNotificationCommand>, CreateNotificationCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateStatusNotificationCommand>, UpdateStatusNotificationCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteNotificationCommand>, DeleteNotificationCommandHandler>();

            // Payment
            services.AddScoped<IRequestHandler<CreatePaymentCommand>, CreatePaymentCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePaymentCommand>, UpdatePaymentCommandHandler>();

            // Promotion
            services.AddScoped<IRequestHandler<CreatePromotionCommand>, CreatePromotionCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePromotionCommand>, UpdatePromotionCommandHandler>();
            services.AddScoped<IRequestHandler<DeletePromotionCommand>, DeletePromotionCommandHandler>();

            // Review
            services.AddScoped<IRequestHandler<CreateReviewCommand>, CreateReviewCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteReviewCommand>, DeleteReviewCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateReviewCommand>, UpdateReviewCommandHandler>();

            // Service Images
            services.AddScoped<IRequestHandler<CreateServiceImageCommand>, CreateServiceImageCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateServiceImageCommand>, UpdateServiceImageCommandHandler>();

            // Service
            services.AddScoped<IRequestHandler<CreateServiceCommand>, CreateServiceCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateServiceCommand>, UpdateServiceCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteServiceCommand>, DeleteServiceCommandHandler>();

            // Technician
            services.AddScoped<IRequestHandler<CreateTechnicianCommand>, CreateTechnicianCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateTechnicianCommand>, UpdateTechnicianCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteTechnicianCommand>, DeleteTechnicianCommandHandler>();

            // Work Schedules
            services.AddScoped<IRequestHandler<CreateWorkScheduleCommand>, CreateWorkScheduleCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateWorkScheduleCommand>, UpdateWorkScheduleCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteWorkScheduleCommand>, DeleteWorkScheduleCommandHandler>();

            // Refresh Tokens
            services.AddScoped<IRequestHandler<CreateRefreshTokenCommand>, CreateRefreshTokenCommandHandler>();

            // File
            services.AddScoped<IRequestHandler<UploadFileCommand, string>, UploadFileCommandHandler>();

            // Service Options
            services.AddScoped<IRequestHandler<CreateServiceOptionCommand>, CreateServiceOptionCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateServiceOptionCommand>, UpdateServiceOptionCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteServiceOptionCommand>, DeleteServiceOptionCommandHandler>();

            // Booking Details
            services.AddScoped<IRequestHandler<CreateBookingDetailCommand>, CreateBookingDetailCommandHandler>();

            return services;
        }

        public static IServiceCollection AddNotificationHandlers(this IServiceCollection services)
        {
            // Fanout
            services.AddScoped<IFanoutEventHandler, FanoutEventHandler>();

            // User
            services.AddScoped<INotificationHandler<UserCreatedEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserUpdatedEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserDeletedEvent>, UserEventHandler>();
            /*            services.AddScoped<INotificationHandler<PasswordChangedEvent>, UserEventHandler>();*/

            return services;
        }

        public static IServiceCollection AddApiUser(this IServiceCollection services)
        {
            // User
            services.AddScoped<IUser, ApiUser>();

            return services;
        }

        public static IServiceCollection AddBunnyCDN(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<BunnyCDNSettings>()
                .Bind(configuration.GetSection("BunnyCDN"))
                .ValidateOnStart();

            return services;
        }

        public static IServiceCollection AddImageKit(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<ImageKitSettings>()
                .Bind(configuration.GetSection("ImageKit"))
                .ValidateOnStart();

            return services;
        }
    }
}
