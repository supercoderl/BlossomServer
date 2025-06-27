using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Reviews.CreateReview
{
    public sealed class CreateReviewCommand : CommandBase, IRequest
    {
        private static readonly CreateReviewCommandValidation s_validation = new();

        public Guid ReviewId { get; }
        public Guid BookingId { get; }
        public Guid CustomerId { get; }
        public Guid TechnicianId { get; }
        public int Rating { get; }
        public string Comment { get; }

        public CreateReviewCommand(
            Guid reviewId,
            Guid bookingId,
            Guid customerId,
            Guid technicianId,
            int rating,
            string comment
        ) : base(Guid.NewGuid())
        {
            ReviewId = reviewId;
            BookingId = bookingId;
            CustomerId = customerId;
            TechnicianId = technicianId;
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
