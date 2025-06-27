using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Reviews
{
    public sealed class ReviewViewModel
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid TechnicianId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public static ReviewViewModel FromReview(Review review)
        {
            return new ReviewViewModel
            {
                Id = review.Id,
                BookingId = review.BookingId,
                CustomerId = review.CustomerId,
                TechnicianId = review.TechnicianId,
                Rating = review.Rating,
                Comment = review.Comment ?? string.Empty,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
