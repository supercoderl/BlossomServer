using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.ServiceImages
{
    public sealed class ServiceImageViewModel
    {
        public Guid Id { get; set; } 
        public string ImageName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ServiceName { get; set; } = string.Empty;

        public static ServiceImageViewModel FromServiceImage(ServiceImage serviceImage)
        {
            return new ServiceImageViewModel
            {
                Id = serviceImage.Id,
                ImageName = serviceImage.ImageName,
                ImageUrl = serviceImage.ImageUrl,
                Description = serviceImage.Description,
                CreatedAt = serviceImage.CreatedAt,
                ServiceId = serviceImage.ServiceId,
                ServiceName = serviceImage.Service?.Name ?? string.Empty
            };
        }
    }
}
