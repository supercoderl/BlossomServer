using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Bookings.GetAll
{
    public sealed record GetAllBookingsQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<BookingViewModel>>;
}
