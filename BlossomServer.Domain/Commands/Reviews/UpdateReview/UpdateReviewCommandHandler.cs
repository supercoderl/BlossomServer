using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Review;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Reviews.UpdateReview
{
    public sealed class UpdateReviewCommandHandler : CommandHandlerBase, IRequestHandler<UpdateReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;

        public UpdateReviewCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IReviewRepository reviewRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;

            var review = await _reviewRepository.GetByIdAsync(request.ReviewId);

            if (review == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no review with ID {request.ReviewId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            review.SetRating(request.Rating);
            review.SetComment(request.Comment);

            _reviewRepository.Update(review);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ReviewUpdatedEvent(review.Id));
            }
        }
    }
}
