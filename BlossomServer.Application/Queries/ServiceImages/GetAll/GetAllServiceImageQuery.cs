using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.ServiceImages.GetAll
{
    public sealed record GetAllServiceImagesQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<ServiceImageViewModel>>;
}
