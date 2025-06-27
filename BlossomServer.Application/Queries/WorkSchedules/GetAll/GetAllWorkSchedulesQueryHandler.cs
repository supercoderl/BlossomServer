using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.WorkSchedules;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var workSchedulesQuery = _workScheduleRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await workSchedulesQuery.CountAsync(cancellationToken);

            workSchedulesQuery = workSchedulesQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var workSchedules = await workSchedulesQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(workSchedule => WorkScheduleViewModel.FromWorkSchedule(workSchedule))
                .ToListAsync(cancellationToken);

            return new PagedResult<WorkScheduleViewModel>(
                totalCount, workSchedules, request.Query.Page, request.Query.PageSize);
        }
    }
}
