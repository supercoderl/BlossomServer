using BlossomServer.SharedKernel.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlossomServer.Domain.Entities
{
    public class RefreshToken : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("UserId")]
        [InverseProperty("RefreshTokens")]
        public virtual User? User { get; set; }

        public RefreshToken(
            Guid id,
            Guid userId,
            string token,
            DateTime expiryDate
        ) : base(id)
        {
            UserId = userId;
            Token = token;
            ExpiryDate = expiryDate;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetUserId(Guid userId) { UserId = userId; }
        public void SetToken(string token) { Token = token; }
        public void SetExpiryDate(DateTime expiryDate) { ExpiryDate = expiryDate; }
    }
}
