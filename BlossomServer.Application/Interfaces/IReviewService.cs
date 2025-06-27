using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IReviewService
    {
        public Task<ReviewViewModel?> GetReviewByReviewIdAsync(Guid reviewId);

        public Task<PagedResult<ReviewViewModel>> GetAllReviewsAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task<Guid> CreateReviewAsync(CreateReviewViewModel review);
        public Task UpdateReviewAsync(UpdateReviewViewModel review);
        public Task DeleteReviewAsync(Guid reviewId);
    }
}
