using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

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
            var results = await _promotionRepository.GetAllPromotionsBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var promotions = results.Select(p => PromotionViewModel.FromPromotion(p)).ToList();

            return new PagedResult<PromotionViewModel>(results.Count(), promotions, request.Query.Page, request.Query.PageSize);
        }
    }
}
