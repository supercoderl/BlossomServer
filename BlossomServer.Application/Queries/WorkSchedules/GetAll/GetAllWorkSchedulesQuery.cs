using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.WorkSchedules;
using MediatR;

namespace BlossomServer.Application.Queries.WorkSchedules.GetAll
{
    public sealed record GetAllWorkSchedulesQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<WorkScheduleViewModel>>;
}
