using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Reviews
{
    public sealed record UpdateReviewViewModel
    (
        Guid ReviewId,
        Guid RatingId,
        int Rating,
        string Comment
    );
}
