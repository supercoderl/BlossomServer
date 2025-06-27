using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Promotions.GetAll
{
    public sealed record GetAllPromotionsQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<PromotionViewModel>>;
}
