using BlossomServer.Application.Behaviors;
using BlossomServer.Application.EventHandler;
using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Blogs.GetAll;
using BlossomServer.Application.Queries.Blogs.GetById;
using BlossomServer.Application.Queries.Bookings.GetAll;
using BlossomServer.Application.Queries.Bookings.GetAllTimeSlotForTechnician;
using BlossomServer.Application.Queries.Bookings.GetById;
using BlossomServer.Application.Queries.Bookings.GetScheduleByDate;
using BlossomServer.Application.Queries.Categories.GetAll;
using BlossomServer.Application.Queries.Categories.GetById;
using BlossomServer.Application.Queries.Contacts.GetAll;
using BlossomServer.Application.Queries.Contacts.GetAllByEmail;
using BlossomServer.Application.Queries.Dashboards.Admin;
using BlossomServer.Application.Queries.Messages.CheckBot;
using BlossomServer.Application.Queries.Messages.FindConversation;
using BlossomServer.Application.Queries.Messages.GetAll;
using BlossomServer.Application.Queries.Notifications.GetAll;
using BlossomServer.Application.Queries.Payments.GetAll;
using BlossomServer.Application.Queries.Promotions.CheckByCode;
using BlossomServer.Application.Queries.Promotions.GetAll;
using BlossomServer.Application.Queries.Promotions.GetById;
using BlossomServer.Application.Queries.Reviews.GetAll;
using BlossomServer.Application.Queries.Reviews.GetById;
using BlossomServer.Application.Queries.ServiceImages.GetAll;
using BlossomServer.Application.Queries.Services.GetAll;
using BlossomServer.Application.Queries.Services.GetAllBySQL;
using BlossomServer.Application.Queries.Services.GetById;
using BlossomServer.Application.Queries.Technicians.GetAll;
using BlossomServer.Application.Queries.Technicians.GetById;
using BlossomServer.Application.Queries.Users.GetAll;
using BlossomServer.Application.Queries.Users.GetById;
using BlossomServer.Application.Queries.WorkSchedules.GetAll;
using BlossomServer.Application.Queries.WorkSchedules.GetById;
using BlossomServer.Application.Services;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Messages;
using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Application.ViewModels.WorkSchedules;
using BlossomServer.Domain.Entities;
using BlossomServer.Shared.Events.Admin;
using BlossomServer.Shared.Events.Message;
using BlossomServer.Shared.Events.Notification;
using BlossomServer.Shared.Events.ServiceImage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlossomServer.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<ITechnicianService, TechnicianService>();
            services.AddScoped<IWorkScheduleService, WorkScheduleService>();
            services.AddScoped<IServiceImageService, ServiceImageService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IServiceOptionService, ServiceOptionService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ISignalRService, SignalRService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISubscriberService, SubscriberService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactResponseService, ContactResponseService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<INotificationService, NotificationService>();    

            return services;
        }

        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            // User
            services.AddScoped<IRequestHandler<GetUserByIdQuery, UserViewModel?>, GetUserByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllUsersQuery, PagedResult<UserViewModel>>, GetAllUsersQueryHandler>();

            // Booking
            services.AddScoped<IRequestHandler<GetBookingByIdQuery, BookingViewModel?>, GetBookingByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllBookingsQuery, PagedResult<BookingViewModel>>, GetAllBookingsQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllTimeSlotForTechnicianQuery, IEnumerable<ScheduleSlot>>, GetAllTimeSlotForTechnicianQueryHandler>();
            services.AddScoped<IRequestHandler<GetScheduleByDateQuery, IEnumerable<object>>, GetScheduleByDateQueryHandler>();

            // Category
            services.AddScoped<IRequestHandler<GetCategoryByIdQuery, CategoryViewModel?>, GetCategoryByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllCategoriesQuery, PagedResult<CategoryViewModel>>, GetAllCategoriesQueryHandler>();

            // Promotion
            services.AddScoped<IRequestHandler<GetPromotionByIdQuery, PromotionViewModel?>, GetPromotionByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllPromotionsQuery, PagedResult<PromotionViewModel>>, GetAllPromotionsQueryHandler>();
            services.AddScoped<IRequestHandler<CheckPromotionByCodeQuery, object>, CheckPromotionByCodeQueryHandler>();

            // Review
            services.AddScoped<IRequestHandler<GetReviewByIdQuery, ReviewViewModel?>, GetReviewByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllReviewsQuery, PagedResult<ReviewViewModel>>, GetAllReviewsQueryHandler>();

            // Service
            services.AddScoped<IRequestHandler<GetServiceByIdQuery, ServiceViewModel?>, GetServiceByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllServicesQuery, PagedResult<ServiceViewModel>>, GetAllServicesQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllServicesBySQLQuery, PagedResult<ServiceViewModel>>, GetAllServicesBySQLQueryHandler>();

            // Technician
            services.AddScoped<IRequestHandler<GetTechnicianByIdQuery, TechnicianViewModel?>, GetTechnicianByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllTechniciansQuery, PagedResult<TechnicianViewModel>>, GetAllTechniciansQueryHandler>();

            // WorkSchedule
            services.AddScoped<IRequestHandler<GetWorkScheduleByIdQuery, WorkScheduleViewModel?>, GetWorkScheduleByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllWorkSchedulesQuery, PagedResult<WorkScheduleViewModel>>, GetAllWorkSchedulesQueryHandler>();

            // ServiceImage
            services.AddScoped<IRequestHandler<GetAllServiceImagesQuery, PagedResult<ServiceImageViewModel>>, GetAllServiceImagesQueryHandler>();

            // Payment
            services.AddScoped<IRequestHandler<GetAllPaymentsQuery, PagedResult<PaymentViewModel>>, GetAllPaymentsQueryHandler>();

            // Messages
            services.AddScoped<IRequestHandler<CheckBotQuery, bool>, CheckBotQueryHandler>();
            services.AddScoped<IRequestHandler<FindConversationIdQuery, Guid>, FindConversationIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllMessagesQuery, PagedResult<MessageViewModel>>, GetAllMessagesQueryHandler>();

            // Contact
            services.AddScoped<IRequestHandler<GetAllContactsQuery, PagedResult<ContactViewModel>>, GetAllContactsQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllContactsByEmailQuery, PagedResult<ContactViewModel>>, GetAllContactsByEmailQueryHandler>();

            // Blog
            services.AddScoped<IRequestHandler<GetAllBlogsQuery, PagedResult<BlogViewModel>>, GetAllBlogsQueryHandler>();
            services.AddScoped<IRequestHandler<GetBlogByIdQuery, BlogViewModel?>, GetBlogByIdQueryHandler>();

            // Admin
            services.AddScoped<IRequestHandler<GetBusinessAnalyticsQuery, object>, GetBusinessAnalyticsQueryHandler>();

            // Notification
            services.AddScoped<IRequestHandler<GetAllNotificationsQuery, PagedResult<NotificationViewModel>>, GetAllNotificationsQueryHandler>();

            return services;
        }

        public static IServiceCollection AddSortProviders(this IServiceCollection services)
        {
            services.AddScoped<ISortingExpressionProvider<UserViewModel, User>, UserViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<BookingViewModel, Booking>, BookingViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<CategoryViewModel, Category>, CategoryViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<PromotionViewModel, Promotion>, PromotionViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<ReviewViewModel, Review>, ReviewViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<ServiceViewModel, Service>, ServiceViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<TechnicianViewModel, Technician>, TechnicianViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<WorkScheduleViewModel, WorkSchedule>, WorkScheduleViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<ServiceImageViewModel, ServiceImage>, ServiceImageViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<PaymentViewModel, Payment>, PaymentViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<MessageViewModel, Message>, MessageViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<ContactViewModel, Contact>, ContactViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<BlogViewModel, Blog>, BlogViewModelSortProvider>();
            services.AddScoped<ISortingExpressionProvider<NotificationViewModel, Notification>, NotificationViewModelSortProvider>();

            return services;
        }

        public static IServiceCollection AddNotificationHandlersApplication(this IServiceCollection services)
        {
            // Service Image
            services.AddScoped<INotificationHandler<ServiceImageCreatedEvent>, ServiceImageEventHandler>();
            services.AddScoped<INotificationHandler<ServiceImageUploadProgressEvent>, ServiceImageEventHandler>();

            // Message
            services.AddScoped<INotificationHandler<MessageAnswerEvent>, MessageAnswerEventHandler>();

            // Admin
            services.AddScoped<INotificationHandler<NotificationCreatedEvent>, NotificationRequiredEventHandler>();

            return services;
        }

        public static IServiceCollection AddTriggerBasedAuditing(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuditContextPipelineBehavior<,>));

            return services;
        }
    }
}
