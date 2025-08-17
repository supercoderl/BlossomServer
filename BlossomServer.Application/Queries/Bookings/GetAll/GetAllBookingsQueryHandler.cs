using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

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
            var results = await _bookingRepository.GetAllBookingsBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var bookings = results.Select(b => BookingViewModel.FromBooking(b)).ToList();

            return new PagedResult<BookingViewModel>(results.Count(), bookings, request.Query.Page, request.Query.PageSize);
        }
    }
}
