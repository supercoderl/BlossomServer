using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Technicians
{
    public sealed class TechnicianViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Bio { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int YearsOfExperience { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;

        public static TechnicianViewModel FromTechnician(Technician technician)
        {
            return new TechnicianViewModel
            {
                Id = technician.Id,
                UserId = technician.UserId,
                Bio = technician.Bio ?? string.Empty,
                Rating = technician.Rating,
                YearsOfExperience = technician.YearsOfExperience,
                FullName = technician.User != null ? technician.User.FullName : string.Empty,
                AvatarUrl = technician.User != null ? technician.User.AvatarUrl : string.Empty
            };
        }
    }
}
