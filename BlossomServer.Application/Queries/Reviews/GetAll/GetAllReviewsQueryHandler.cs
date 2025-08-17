using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Reviews.GetAll
{
    public sealed class GetAllReviewsQueryHandler :
        IRequestHandler<GetAllReviewsQuery, PagedResult<ReviewViewModel>>
    {
        private readonly ISortingExpressionProvider<ReviewViewModel, Review> _sortingExpressionProvider;
        private readonly IReviewRepository _reviewRepository;

        public GetAllReviewsQueryHandler(
            IReviewRepository reviewRepository,
            ISortingExpressionProvider<ReviewViewModel, Review> sortingExpressionProvider)
        {
            _reviewRepository = reviewRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<ReviewViewModel>> Handle(
            GetAllReviewsQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _reviewRepository.GetAllReviewsBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var reviews = results.Select(r => ReviewViewModel.FromReview(r)).ToList();

            return new PagedResult<ReviewViewModel>(results.Count(), reviews, request.Query.Page, request.Query.PageSize);
        }
    }
}
