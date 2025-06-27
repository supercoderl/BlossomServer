using FluentValidation;

namespace BlossomServer.Domain.Commands.Reviews.CreateReview
{
    public sealed class CreateReviewCommandValidation : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidation()
        {

        }
    }
}
