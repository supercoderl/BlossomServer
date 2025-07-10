using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Application.Queries.Services.GetAll
{
    public sealed class GetAllServicesQueryHandler :
        IRequestHandler<GetAllServicesQuery, PagedResult<ServiceViewModel>>
    {
        private readonly ISortingExpressionProvider<ServiceViewModel, Service> _sortingExpressionProvider;
        private readonly IServiceRepository _serviceRepository;

        public GetAllServicesQueryHandler(
            IServiceRepository serviceRepository,
            ISortingExpressionProvider<ServiceViewModel, Service> sortingExpressionProvider)
        {
            _serviceRepository = serviceRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<ServiceViewModel>> Handle(
            GetAllServicesQuery request,
            CancellationToken cancellationToken)
        {
            var servicesQuery = _serviceRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Include(x => x.ServiceOptions)
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                servicesQuery = servicesQuery.Where(s => EF.Functions.Like(s.Name, $"%{request.SearchTerm}%"));
            }

            var totalCount = await servicesQuery.CountAsync(cancellationToken);

            servicesQuery = servicesQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var services = await servicesQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(service => ServiceViewModel.FromService(service))
                .ToListAsync(cancellationToken);

            return new PagedResult<ServiceViewModel>(
                totalCount, services, request.Query.Page, request.Query.PageSize);
        }
    }
}
