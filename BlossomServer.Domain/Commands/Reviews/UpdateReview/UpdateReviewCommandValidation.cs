using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Reviews.UpdateReview
{
    public sealed class UpdateReviewCommandValidation : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidation()
        { }
    }
}
