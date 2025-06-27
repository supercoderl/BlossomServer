using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.RefreshToken
{
    public sealed class RefreshTokenCreatedEvent : DomainEvent
    {
        public RefreshTokenCreatedEvent(Guid refreshTokenId) : base(refreshTokenId)
        {
            
        }
    }
}
