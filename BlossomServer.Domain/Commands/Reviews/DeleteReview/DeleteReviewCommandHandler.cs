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

namespace BlossomServer.Domain.Commands.Reviews.DeleteReview
{
    public sealed class DeleteReviewCommandHandler : CommandHandlerBase, IRequestHandler<DeleteReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteReviewCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IReviewRepository reviewRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
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

            _reviewRepository.Remove(review);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ReviewDeletedEvent(review.Id));
            }
        }
    }
}
