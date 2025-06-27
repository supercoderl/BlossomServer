using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Reviews.DeleteReview
{
    public sealed class DeleteReviewCommand : CommandBase, IRequest
    {
        private static readonly DeleteReviewCommandValidation s_validation = new();

        public Guid ReviewId { get; }

        public DeleteReviewCommand(Guid reviewId) : base(Guid.NewGuid())
        {
            ReviewId = reviewId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
