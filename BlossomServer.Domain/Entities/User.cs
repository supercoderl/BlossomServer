using BlossomServer.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlossomServer.Domain.Entities
{
    public class User : Entity<Guid>
    {
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string AvatarUrl { get; private set; }
        public string? CoverPhotoUrl { get; private set; }
        public Gender Gender { get; private set; }
        public string? Website { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public UserRole Role { get; private set; }
        public UserStatus Status { get; private set; }
        public DateTimeOffset? LastLoggedinDate { get; private set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool IsBot => Role == UserRole.Bot;

        [InverseProperty("User")]
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [InverseProperty("User")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        [InverseProperty("User")]
        public virtual Technician? Technician { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        [InverseProperty("Customer")]
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        [InverseProperty("Sender")]
        public virtual ICollection<Message> SenderMessages { get; set; } = new List<Message>();

        [InverseProperty("Recipient")]
        public virtual ICollection<Message> RecipientMessages { get; set; } = new List<Message>();

        [InverseProperty("User")]
        public virtual ICollection<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();

        [InverseProperty("User")]
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

        [InverseProperty("User")]
        public virtual ICollection<ContactResponse> ContactResponses { get; set; } = new List<ContactResponse>();

        [InverseProperty("User")]
        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

        public User(
            Guid id,
            string password,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string avatarUrl,
            string? coverPhotoUrl,
            Gender gender,
            string? website,
            DateOnly dateOfBirth,
            UserRole role,
            UserStatus status = UserStatus.Active
        ) : base(id)
        {
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            AvatarUrl = avatarUrl;
            CoverPhotoUrl = coverPhotoUrl;
            Gender = gender;
            Website = website;
            DateOfBirth = dateOfBirth;
            Role = role;
            Status = status;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void SetLastLoggedIn(DateTimeOffset? lastLoggedinDate)
        {
            LastLoggedinDate = lastLoggedinDate;
        }

        public void SetInactive()
        {
            Status = UserStatus.Inactive;
        }

        public void SetActive()
        {
            Status = UserStatus.Active;
        }

        public void SetAvatarUrl(string avatarUrl) { AvatarUrl = avatarUrl; }

        public void SetCoverPhotoUrl(string? coverPhotoUrl) { CoverPhotoUrl = coverPhotoUrl; }

        public void SetGender(Gender gender) { Gender = gender; }

        public void SetWebsite(string? website) { Website = website; }

        public void SetDateOfBirth(DateOnly dateOfBirth) { DateOfBirth = dateOfBirth; }

        public void SetRole(UserRole role) { Role = role; }

        public void SetTechnician(Technician? technician) { Technician = technician; }
    }
}
