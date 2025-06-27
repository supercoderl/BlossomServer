using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Reviews.DeleteReview
{
    public sealed class DeleteReviewCommandValidation : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidation()
        {
            
        }
    }
}
