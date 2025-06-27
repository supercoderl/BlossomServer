using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var reviewsQuery = _reviewRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await reviewsQuery.CountAsync(cancellationToken);

            reviewsQuery = reviewsQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var reviews = await reviewsQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(review => ReviewViewModel.FromReview(review))
                .ToListAsync(cancellationToken);

            return new PagedResult<ReviewViewModel>(
                totalCount, reviews, request.Query.Page, request.Query.PageSize);
        }
    }
}
