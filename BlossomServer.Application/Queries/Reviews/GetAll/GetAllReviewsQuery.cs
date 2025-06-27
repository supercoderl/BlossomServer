using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Reviews.GetAll
{
    public sealed record GetAllReviewsQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<ReviewViewModel>>;
}
