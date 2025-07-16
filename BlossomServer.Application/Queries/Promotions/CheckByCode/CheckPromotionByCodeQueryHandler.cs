using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Promotions.CheckByCode
{
    public sealed class CheckPromotionByCodeQueryHandler :
            IRequestHandler<CheckPromotionByCodeQuery, object>
    {
        private readonly IMediatorHandler _bus;
        private readonly IPromotionRepository _promotionRepository;

        public CheckPromotionByCodeQueryHandler(IPromotionRepository promotionRepository, IMediatorHandler bus)
        {
            _promotionRepository = promotionRepository;
            _bus = bus;
        }

        public async Task<object> Handle(CheckPromotionByCodeQuery request, CancellationToken cancellationToken)
        {
            var promotion = await _promotionRepository.CheckByCode(request.code);

            return new
            {
                isValid = promotion != null,
                value = promotion != null ? promotion.DiscountValue : 0,
                type = promotion != null ? promotion.DiscountType : Domain.Enums.DiscountType.Percent
            };
        }
    }
}
