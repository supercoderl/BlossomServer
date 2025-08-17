using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Contact : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [NotMapped]
        public bool HasResponse { get; private set; }

        [InverseProperty("Contact")]
        public virtual ContactResponse? ContactResponse { get; set; }

        public Contact(
            Guid id,
            string name,
            string email,
            string message
        ) : base(id)
        {
            Name = name;
            Email = email;
            Message = message;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetName(string name) { Name = name; }
        public void SetEmail(string email) { Email = email; }
        public void SetMessage(string message) { Message = message; }
        public void SetCreatedAt(DateTime createdAt) { CreatedAt = createdAt; }
        public void SetHasResponse(bool hasResponse) { HasResponse = hasResponse; }
        public void SetContactResponse(ContactResponse? contactResponse) { ContactResponse = contactResponse; }
    }
}
