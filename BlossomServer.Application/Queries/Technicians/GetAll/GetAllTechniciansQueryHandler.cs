using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Application.Queries.Technicians.GetAll
{
    public sealed class GetAllTechniciansQueryHandler :
        IRequestHandler<GetAllTechniciansQuery, PagedResult<TechnicianViewModel>>
    {
        private readonly ISortingExpressionProvider<TechnicianViewModel, Technician> _sortingExpressionProvider;
        private readonly ITechnicianRepository _technicianRepository;

        public GetAllTechniciansQueryHandler(
            ITechnicianRepository technicianRepository,
            ISortingExpressionProvider<TechnicianViewModel, Technician> sortingExpressionProvider)
        {
            _technicianRepository = technicianRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<TechnicianViewModel>> Handle(
            GetAllTechniciansQuery request,
            CancellationToken cancellationToken)
        {
            var techniciansQuery = _technicianRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Include(x => x.User)
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await techniciansQuery.CountAsync(cancellationToken);

            techniciansQuery = techniciansQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var technicians = await techniciansQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(technician => TechnicianViewModel.FromTechnician(technician))
                .ToListAsync(cancellationToken);

            return new PagedResult<TechnicianViewModel>(
                totalCount, technicians, request.Query.Page, request.Query.PageSize);
        }
    }
}
