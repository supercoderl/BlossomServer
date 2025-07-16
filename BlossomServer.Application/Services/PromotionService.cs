using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Promotions.CheckByCode;
using BlossomServer.Application.Queries.Promotions.GetAll;
using BlossomServer.Application.Queries.Promotions.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Promotions.CreatePromotion;
using BlossomServer.Domain.Commands.Promotions.DeletePromotion;
using BlossomServer.Domain.Commands.Promotions.UpdatePromotion;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IMediatorHandler _bus;

        public PromotionService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<object> CheckPromotionAsync(string code)
        {
            return await _bus.QueryAsync(new CheckPromotionByCodeQuery(code));
        }

        public async Task<Guid> CreatePromotionAsync(CreatePromotionViewModel promotion)
        {
            var promotionId = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreatePromotionCommand(
                promotionId,
                promotion.Code,
                promotion.Description,
                promotion.DiscountType,
                promotion.DiscountValue,
                promotion.MinimumSpend,
                promotion.StartDate,
                promotion.EndDate,
                promotion.MaxUsage,
                0,
                true
            ));

            return promotionId;
        }

        public async Task DeletePromotionAsync(Guid promotionId)
        {
            await _bus.SendCommandAsync(new DeletePromotionCommand(promotionId));
        }

        public async Task<PagedResult<PromotionViewModel>> GetAllPromotionsAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllPromotionsQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<PromotionViewModel?> GetPromotionByPromotionIdAsync(Guid promotionId)
        {
            return await _bus.QueryAsync(new GetPromotionByIdQuery(promotionId));
        }

        public async Task UpdatePromotionAsync(UpdatePromotionViewModel promotion)
        {
            await _bus.SendCommandAsync(new UpdatePromotionCommand(
                promotion.PromotionId,
                promotion.Code,
                promotion.Description,
                promotion.DiscountType,
                promotion.DiscountValue,
                promotion.MinimumSpend,
                promotion.CurrentUsage,
                promotion.StartDate,
                promotion.EndDate,
                promotion.MaxUsage,
                promotion.IsActive
            ));
        }
    }
}
