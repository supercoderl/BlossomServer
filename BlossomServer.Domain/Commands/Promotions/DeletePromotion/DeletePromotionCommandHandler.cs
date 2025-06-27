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

namespace BlossomServer.Domain.Commands.Promotions.DeletePromotion
{
    public sealed class DeletePromotionCommandHandler : CommandHandlerBase, IRequestHandler<DeletePromotionCommand>
    {
        private readonly IPromotionRepository _promotionRepository;

        public DeletePromotionCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IPromotionRepository promotionRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;   

            var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId);

            if(promotion == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Promotion not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            _promotionRepository.Remove(promotion);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new PromotionDeletedEvent(request.PromotionId));
            }
        }
    }
}
