using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.PasswordResetToken
{
    public sealed class PasswordResetTokenCreatedEvent : DomainEvent
    {
        public PasswordResetTokenCreatedEvent(Guid passwordResetTokenId) : base(passwordResetTokenId)
        {
            
        }
    }
}
