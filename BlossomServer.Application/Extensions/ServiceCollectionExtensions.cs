using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Users.GetAll;
using BlossomServer.Application.Queries.Users.GetById;
using BlossomServer.Application.Services;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Entities;
using BlossomServer.Application.Queries.Bookings.GetById;
using BlossomServer.Application.Queries.Bookings.GetAll;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.Queries.Categories.GetById;
using BlossomServer.Application.Queries.Categories.GetAll;
using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.Queries.Promotions.GetById;
using BlossomServer.Application.Queries.Promotions.GetAll;
using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.Queries.Reviews.GetById;
using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.Queries.Reviews.GetAll;
using BlossomServer.Application.Queries.Services.GetById;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.Queries.Services.GetAll;
using BlossomServer.Application.Queries.Technicians.GetById;
using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Application.Queries.Technicians.GetAll;
using BlossomServer.Application.Queries.WorkSchedules.GetById;
using BlossomServer.Application.Queries.WorkSchedules.GetAll;
using BlossomServer.Application.ViewModels.WorkSchedules;
using BlossomServer.Application.Queries.ServiceImages.GetAll;
using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.Queries.Payments.GetAll;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.Queries.Bookings.GetAllTimeSlotForTechnician;

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

            // Category
            services.AddScoped<IRequestHandler<GetCategoryByIdQuery, CategoryViewModel?>, GetCategoryByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllCategoriesQuery, PagedResult<CategoryViewModel>>, GetAllCategoriesQueryHandler>();

            // Promotion
            services.AddScoped<IRequestHandler<GetPromotionByIdQuery, PromotionViewModel?>, GetPromotionByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllPromotionsQuery, PagedResult<PromotionViewModel>>, GetAllPromotionsQueryHandler>();

            // Review
            services.AddScoped<IRequestHandler<GetReviewByIdQuery, ReviewViewModel?>, GetReviewByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllReviewsQuery, PagedResult<ReviewViewModel>>, GetAllReviewsQueryHandler>();

            // Service
            services.AddScoped<IRequestHandler<GetServiceByIdQuery, ServiceViewModel?>, GetServiceByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllServicesQuery, PagedResult<ServiceViewModel>>, GetAllServicesQueryHandler>();

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

            return services;
        }
    }
}
