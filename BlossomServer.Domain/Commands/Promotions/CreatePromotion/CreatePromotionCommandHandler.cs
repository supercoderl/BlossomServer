using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Promotion;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Promotions.CreatePromotion
{
    public sealed class CreatePromotionCommandHandler : CommandHandlerBase, IRequestHandler<CreatePromotionCommand>
    {
        private readonly IPromotionRepository _promotionRepository;

        public CreatePromotionCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IPromotionRepository promotionRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            if (!TimeZoneHelper.TryParseLocalDateTime(request.StartDate, out var startDate) || !TimeZoneHelper.TryParseLocalDateTime(request.EndDate, out var endDate))
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"Date is not correct format",
                    ErrorCodes.InsufficientPermissions
                ));
                return;
            }

            var promotion = new Entities.Promotion(
                request.PromotionId,
                request.Code,
                request.Description,
                request.DiscountType,
                request.DiscountValue,
                request.MinimumSpend,
                startDate,
                endDate,
                request.MaxUsage,
                request.CurrentUsage,
                request.IsActive
            );

            _promotionRepository.Add(promotion);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new PromotionCreatedEvent(promotion.Id));
            }
        }
    }
}
