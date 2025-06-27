using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IPromotionService
    {
        public Task<PromotionViewModel?> GetPromotionByPromotionIdAsync(Guid promotionId);

        public Task<PagedResult<PromotionViewModel>> GetAllPromotionsAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task<Guid> CreatePromotionAsync(CreatePromotionViewModel promotion);
        public Task UpdatePromotionAsync(UpdatePromotionViewModel promotion);
        public Task DeletePromotionAsync(Guid promotionId);
    }
}
