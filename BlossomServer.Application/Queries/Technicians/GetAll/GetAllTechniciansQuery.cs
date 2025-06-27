using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Technicians;
using MediatR;

namespace BlossomServer.Application.Queries.Technicians.GetAll
{
    public sealed record GetAllTechniciansQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<TechnicianViewModel>>;
}
