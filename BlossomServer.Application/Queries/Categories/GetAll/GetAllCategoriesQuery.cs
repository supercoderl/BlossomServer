using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Categories.GetAll
{
    public sealed record GetAllCategoriesQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<CategoryViewModel>>;
}
