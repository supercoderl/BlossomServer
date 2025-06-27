using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.CreateUser
{
    public sealed class CreateUserCommand : CommandBase
    {
        private static readonly CreateUserCommandValidation s_validation = new();

        public Guid UserId { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Password { get; }
        public string PhoneNumber { get; }
        public string AvatarUrl { get; }
        public Gender Gender { get; }
        public DateOnly DateOfBirth { get; }
        public UserRole Role { get; }

        public CreateUserCommand(
            Guid userId,
            string email,
            string firstName,
            string lastName,
            string password,
            string phoneNumber,
            string avatarUrl,
            Gender gender,
            DateOnly dateOfBirth,
            UserRole role
        ) : base(userId)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            PhoneNumber = phoneNumber;
            AvatarUrl = avatarUrl;
            Gender = gender;
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
