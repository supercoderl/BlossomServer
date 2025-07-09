using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Application.Queries.Bookings.GetAll
{
    public sealed class GetAllBookingsQueryHandler :
        IRequestHandler<GetAllBookingsQuery, PagedResult<BookingViewModel>>
    {
        private readonly ISortingExpressionProvider<BookingViewModel, Booking> _sortingExpressionProvider;
        private readonly IBookingRepository _bookingRepository;

        public GetAllBookingsQueryHandler(
            IBookingRepository bookingRepository,
            ISortingExpressionProvider<BookingViewModel, Booking> sortingExpressionProvider)
        {
            _bookingRepository = bookingRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<BookingViewModel>> Handle(
            GetAllBookingsQuery request,
            CancellationToken cancellationToken)
        {
            var bookingsQuery = _bookingRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Include(x => x.BookingDetails)
                    .ThenInclude(x => x.ServiceOption)
                    .ThenInclude(x => x.Service)
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await bookingsQuery.CountAsync(cancellationToken);

            bookingsQuery = bookingsQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var bookings = await bookingsQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(booking => BookingViewModel.FromBooking(booking))
                .ToListAsync(cancellationToken);

            return new PagedResult<BookingViewModel>(
                totalCount, bookings, request.Query.Page, request.Query.PageSize);
        }
    }
}
