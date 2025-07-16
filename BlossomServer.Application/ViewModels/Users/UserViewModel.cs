using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Users
{
    public sealed class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string? CoverPhotoUrl { get; set; }
        public Gender Gender { get; set; }
        public string? Website { get; set; }
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; }
        public DateOnly DateOfBirth { get; set; }   
        public TechnicianViewModel? TechnicianInfo { get; set; }
        public string? Device { get; set; }
        public Guid? CurrentRoomId { get; set; }
        public string? ConnectionId { get; set; }

        public static UserViewModel FromUser(User user, string? device, Guid? currentRoomId, string? connectionId)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                CoverPhotoUrl = user.CoverPhotoUrl,
                Gender = user.Gender,
                Website = user.Website,
                Role = user.Role,
                Status = user.Status,
                DateOfBirth = user.DateOfBirth,
                TechnicianInfo = user.Technician != null ? TechnicianViewModel.FromTechnician(user.Technician) : null,
                Device = device,
                CurrentRoomId = currentRoomId,
                ConnectionId = connectionId
            };
        }
    }
}
