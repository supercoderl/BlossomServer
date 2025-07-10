using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Application.Queries.Promotions.GetAll
{
    public sealed class GetAllPromotionsQueryHandler :
        IRequestHandler<GetAllPromotionsQuery, PagedResult<PromotionViewModel>>
    {
        private readonly ISortingExpressionProvider<PromotionViewModel, Promotion> _sortingExpressionProvider;
        private readonly IPromotionRepository _promotionRepository;

        public GetAllPromotionsQueryHandler(
            IPromotionRepository promotionRepository,
            ISortingExpressionProvider<PromotionViewModel, Promotion> sortingExpressionProvider)
        {
            _promotionRepository = promotionRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<PromotionViewModel>> Handle(
            GetAllPromotionsQuery request,
            CancellationToken cancellationToken)
        {
            var promotionsQuery = _promotionRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                promotionsQuery = promotionsQuery.Where(s => EF.Functions.Like(s.Code, $"%{request.SearchTerm}%"));
            }

            var totalCount = await promotionsQuery.CountAsync(cancellationToken);

            promotionsQuery = promotionsQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var promotions = await promotionsQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(promotion => PromotionViewModel.FromPromotion(promotion))
                .ToListAsync(cancellationToken);

            return new PagedResult<PromotionViewModel>(
                totalCount, promotions, request.Query.Page, request.Query.PageSize);
        }
    }
}
