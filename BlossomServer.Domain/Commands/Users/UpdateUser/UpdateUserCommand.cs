using BlossomServer.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.UpdateUser
{
    public sealed class UpdateUserCommand : CommandBase
    {
        private static readonly UpdateUserCommandValidation s_validation = new();

        public Guid UserId { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PhoneNumber { get; }
        public IFormFile? AvatarFile { get; }
        public IFormFile? CoverPhoto { get; }
        public Gender Gender { get; }
        public string? Website { get; }
        public DateOnly DateOfBirth { get; }
        public UserRole Role { get; }

        public UpdateUserCommand(
            Guid userId,
            string email,
            string firstName,
            string lastName,
            string phoneNumber,
            IFormFile? avatarFile,
            IFormFile? coverPhoto,
            Gender gender,
            string? website,
            DateOnly dateOfBirth,
            UserRole role
        ) : base(userId)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            AvatarFile = avatarFile;
            CoverPhoto = coverPhoto;
            Gender = gender;
            Website = website;
            DateOfBirth = dateOfBirth;
            Role = role;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
