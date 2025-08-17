using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class ContactResponse : Entity<Guid>
    {
        public Guid ContactId { get; private set; }
        public string ResponseText { get; private set; }
        public Guid ResponderId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("ContactId")]
        [InverseProperty("ContactResponse")]
        public virtual Contact? Contact { get; set; }

        [ForeignKey("ResponderId")]
        [InverseProperty("ContactResponses")]
        public virtual User? User { get; set; }

        public ContactResponse(
            Guid id,
            Guid contactId,
            string responseText,
            Guid responderId
        ) : base(id)
        {
            ContactId = contactId;
            ResponseText = responseText;
            ResponderId = responderId;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetContactId(Guid contactId) { ContactId = contactId; }
        public void SetResponseText(string responseText) { ResponseText = responseText; }
        public void SetResponderId(Guid responderId) { ResponderId = responderId; }
        public void SetCreatedAt(DateTime createdAt) { CreatedAt = createdAt; }
        public void SetUser(User? user) { User = user; }
    }
}
