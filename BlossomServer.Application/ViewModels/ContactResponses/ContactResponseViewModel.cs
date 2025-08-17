using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.ContactResponses
{
    public sealed class ContactResponseViewModel
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public string ResponseText { get; set; } = string.Empty;
        public Guid ResponderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserViewModel? Responder { get; set; }

        public static ContactResponseViewModel FromContactResponse(ContactResponse contactResponse)
        {
            return new ContactResponseViewModel
            {
                Id = contactResponse.Id,
                ContactId = contactResponse.ContactId,
                ResponseText = contactResponse.ResponseText,
                ResponderId = contactResponse.ResponderId,
                CreatedAt = contactResponse.CreatedAt,
                Responder = contactResponse.User != null ? UserViewModel.FromUser(contactResponse.User, null, null, null) : null
            };
        }
    }
}
