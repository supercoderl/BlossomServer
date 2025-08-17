using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.WorkSchedules;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.WorkSchedules.GetAll
{
    public sealed class GetAllWorkSchedulesQueryHandler :
        IRequestHandler<GetAllWorkSchedulesQuery, PagedResult<WorkScheduleViewModel>>
    {
        private readonly ISortingExpressionProvider<WorkScheduleViewModel, WorkSchedule> _sortingExpressionProvider;
        private readonly IWorkScheduleRepository _workScheduleRepository;

        public GetAllWorkSchedulesQueryHandler(
            IWorkScheduleRepository workScheduleRepository,
            ISortingExpressionProvider<WorkScheduleViewModel, WorkSchedule> sortingExpressionProvider)
        {
            _workScheduleRepository = workScheduleRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<WorkScheduleViewModel>> Handle(
            GetAllWorkSchedulesQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _workScheduleRepository.GetAllWorkSchedulesBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var workSchedules = results.Select(ws => WorkScheduleViewModel.FromWorkSchedule(ws)).ToList();

            return new PagedResult<WorkScheduleViewModel>(results.Count(), workSchedules, request.Query.Page, request.Query.PageSize);
        }
    }
}
