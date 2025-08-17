using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Blogs.GetAll
{
    public sealed record GetAllBlogsQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        bool IsPublished = true,
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<BlogViewModel>>;
}
