using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Reviews.GetAll;
using BlossomServer.Application.Queries.Reviews.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Reviews.CreateReview;
using BlossomServer.Domain.Commands.Reviews.DeleteReview;
using BlossomServer.Domain.Commands.Reviews.UpdateReview;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMediatorHandler _bus;

        public ReviewService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> CreateReviewAsync(CreateReviewViewModel review)
        {
            var reviewId = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateReviewCommand(
                reviewId,
                review.BookingId,
                review.CustomerId,
                review.TechnicianId,
                review.Rating,
                review.Comment
            ));

            return reviewId;
        }

        public async Task DeleteReviewAsync(Guid reviewId)
        {
            await _bus.SendCommandAsync(new DeleteReviewCommand(reviewId));
        }

        public async Task<PagedResult<ReviewViewModel>> GetAllReviewsAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllReviewsQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<ReviewViewModel?> GetReviewByReviewIdAsync(Guid reviewId)
        {
            return await _bus.QueryAsync(new GetReviewByIdQuery(reviewId));
        }

        public async Task UpdateReviewAsync(UpdateReviewViewModel review)
        {
            await _bus.SendCommandAsync(new UpdateReviewCommand(
                review.ReviewId,
                review.Rating,
                review.Comment
            ));
        }
    }
}
