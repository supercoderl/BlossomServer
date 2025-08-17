using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Technician : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Bio { get; private set; }
        public double Rating { get; private set; }
        public int YearsOfExperience { get; private set; }

        [InverseProperty("Technician")]
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        [InverseProperty("Technician")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [InverseProperty("Technician")]
        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();

        public Technician(
            Guid id,
            Guid userId,
            string bio,
            double rating,
            int yearsOfExperience
        ) : base(id)
        {
            UserId = userId;
            Bio = bio;
            Rating = rating;
            YearsOfExperience = yearsOfExperience;
        }

        public void SetUserId(Guid userId) { UserId = userId; }
        public void SetBio( string bio ) { Bio = bio; }
        public void SetRating( double rating ) { Rating = rating; }
        public void SetYearsOfExperience( int yearsOfExperience ) { YearsOfExperience = yearsOfExperience; }
        public void SetUser(User? user) { User = user; }
    }
}
