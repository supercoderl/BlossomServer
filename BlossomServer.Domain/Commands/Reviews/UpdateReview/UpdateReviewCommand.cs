using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Reviews.UpdateReview
{
    public sealed class UpdateReviewCommand : CommandBase, IRequest
    {
        private static readonly UpdateReviewCommandValidation s_validation = new();

        public Guid ReviewId { get; }
        public int Rating { get; }
        public string Comment { get; }

        public UpdateReviewCommand(
            Guid reviewId,
            int rating,
            string comment
        ) : base(Guid.NewGuid())
        {
            ReviewId = reviewId;
            Rating = rating;
            Comment = comment;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
