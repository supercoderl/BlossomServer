using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var serviceImagesQuery = _serviceImageRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Include(x => x.Service)
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await serviceImagesQuery.CountAsync(cancellationToken);

            serviceImagesQuery = serviceImagesQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var serviceImages = await serviceImagesQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(serviceImage => ServiceImageViewModel.FromServiceImage(serviceImage))
                .ToListAsync(cancellationToken);

            return new PagedResult<ServiceImageViewModel>(
                totalCount, serviceImages, request.Query.Page, request.Query.PageSize);
        }
    }
}
