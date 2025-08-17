using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

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
            var results = await _technicianRepository.GetAllTechniciansBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var technicians = results.Select(t => TechnicianViewModel.FromTechnician(t)).ToList();

            return new PagedResult<TechnicianViewModel>(results.Count(), technicians, request.Query.Page, request.Query.PageSize);
        }
    }
}
