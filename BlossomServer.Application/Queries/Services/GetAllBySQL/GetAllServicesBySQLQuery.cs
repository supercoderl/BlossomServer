using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Services.GetAllBySQL
{
    public sealed record GetAllServicesBySQLQuery(
            PageQuery Query,
            bool IncludeDeleted,
            string SearchTerm = "",
            SortQuery? SortQuery = null) :
            IRequest<PagedResult<ServiceViewModel>>;
}
