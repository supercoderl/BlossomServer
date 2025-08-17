using BlossomServer.Application.ViewModels.ContactResponses;
using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Contacts
{
    public sealed class ContactViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 
        public bool HasResponse { get; set; }
        public ContactResponseViewModel? ContactResponse { get; set; }

        public static ContactViewModel FromContact(Contact contact)
        {
            return new ContactViewModel
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                CreatedAt = contact.CreatedAt,
                HasResponse = contact.HasResponse,
                ContactResponse = contact.ContactResponse != null ? ContactResponseViewModel.FromContactResponse(contact.ContactResponse) : null
            };
        }
    }
}
