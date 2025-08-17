using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class PasswordResetToken : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsUsed { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("UserId")]
        [InverseProperty("PasswordResetTokens")]
        public virtual User? User { get; set; }

        public PasswordResetToken(
            Guid id,
            Guid userId,
            string token
        ) : base(id)
        {
            UserId = userId;
            Token = token;
            ExpirationDate = TimeZoneHelper.GetLocalTimeNow().AddMinutes(15);
            IsUsed = false;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetUserId(Guid userId) { UserId = userId; }
        public void SetToken(string token) { Token = token; }
        public void SetExpirationDate(DateTime expirationDate) { ExpirationDate = expirationDate; }
        public void SetIsUsed(bool isUsed) { IsUsed = isUsed; }
    }
}
