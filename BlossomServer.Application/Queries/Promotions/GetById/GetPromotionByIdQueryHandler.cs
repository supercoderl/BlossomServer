using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.Promotions.GetById
{
    public sealed class GetPromotionByIdQueryHandler :
        IRequestHandler<GetPromotionByIdQuery, PromotionViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly IPromotionRepository _promotionRepository;

        public GetPromotionByIdQueryHandler(IPromotionRepository promotionRepository, IMediatorHandler bus)
        {
            _promotionRepository = promotionRepository;
            _bus = bus;
        }

        public async Task<PromotionViewModel?> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
        {
            var promotion = await _promotionRepository.GetByIdAsync(request.Id);

            if (promotion is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetPromotionByIdQuery),
                        $"Promotion with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return PromotionViewModel.FromPromotion(promotion);
        }
    }
}
