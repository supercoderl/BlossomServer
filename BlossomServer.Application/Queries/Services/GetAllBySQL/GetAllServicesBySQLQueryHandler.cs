using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MassTransit.Initializers;
using MediatR;

namespace BlossomServer.Application.Queries.Services.GetAllBySQL
{
    public sealed class GetAllServicesBySQLQueryHandler :
        IRequestHandler<GetAllServicesBySQLQuery, PagedResult<ServiceViewModel>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ISortingExpressionProvider<ServiceViewModel, Service> _sortingExpressionProvider;

        public GetAllServicesBySQLQueryHandler(
            IServiceRepository serviceRepository,
            ISortingExpressionProvider<ServiceViewModel, Service> sortingExpressionProvider
        )
        {
            _serviceRepository = serviceRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<ServiceViewModel>> Handle(GetAllServicesBySQLQuery request, CancellationToken cancellationToken)
        {
            var results = await _serviceRepository.GetAllServicesBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var services = results.Select(s => ServiceViewModel.FromService(s)).ToList();

            return new PagedResult<ServiceViewModel>(results.Count(), services, request.Query.Page, request.Query.PageSize);
        }
    }
}
