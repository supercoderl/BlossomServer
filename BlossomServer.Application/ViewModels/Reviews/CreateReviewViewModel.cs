using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Reviews
{
    public sealed record CreateReviewViewModel
    (
        Guid BookingId,
        Guid CustomerId,
        Guid TechnicianId,
        int Rating,
        string Comment
    );
}
