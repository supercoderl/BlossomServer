using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Domain.Errors;
using BlossomServer.Shared.Events.Promotion;

namespace BlossomServer.Domain.Commands.Promotions.UpdatePromotion
{
    public sealed class UpdatePromotionCommandHandler : CommandHandlerBase, IRequestHandler<UpdatePromotionCommand>
    {
        private readonly IPromotionRepository _promotionRepository;

        public UpdatePromotionCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IPromotionRepository promotionRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId);

            if (promotion == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Promotion not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            promotion.SetCode(request.Code);
            promotion.SetDescription(request.Description);
            promotion.SetDiscountType(request.DiscountType);
            promotion.SetDiscountValue(request.DiscountValue);
            promotion.SetMinimumSpend(request.MinimumSpend);
            promotion.SetStartDate(request.StartDate);
            promotion.SetEndDate(request.EndDate);
            promotion.SetMaxUsage(request.MaxUsage);
            promotion.SetCurrentUsage(request.CurrentUsage);
            promotion.SetIsActive(request.IsActive);

            _promotionRepository.Update(promotion);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new PromotionUpdatedEvent(promotion.Id));
            }
        }
    }
}
