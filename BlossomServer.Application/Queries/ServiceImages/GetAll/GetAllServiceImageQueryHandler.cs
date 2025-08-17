using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.ServiceImages.GetAll
{
    public sealed class GetAllServiceImagesQueryHandler :
            IRequestHandler<GetAllServiceImagesQuery, PagedResult<ServiceImageViewModel>>
    {
        private readonly ISortingExpressionProvider<ServiceImageViewModel, ServiceImage> _sortingExpressionProvider;
        private readonly IServiceImageRepository _serviceImageRepository;

        public GetAllServiceImagesQueryHandler(
            IServiceImageRepository serviceImageRepository,
            ISortingExpressionProvider<ServiceImageViewModel, ServiceImage> sortingExpressionProvider)
        {
            _serviceImageRepository = serviceImageRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<ServiceImageViewModel>> Handle(
            GetAllServiceImagesQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _serviceImageRepository.GetAllServiceImagesBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var serviceImages = results.Select(si => ServiceImageViewModel.FromServiceImage(si)).ToList();

            return new PagedResult<ServiceImageViewModel>(results.Count(), serviceImages, request.Query.Page, request.Query.PageSize);
        }
    }
}
