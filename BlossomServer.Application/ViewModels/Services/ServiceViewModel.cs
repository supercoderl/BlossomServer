using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Services
{
    public sealed class ServiceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public Guid CategoryId { get; private set; }
        public decimal Price { get; private set; }
        public int DurationMinutes { get; private set; }
        public string RepresentativeImage { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public static ServiceViewModel FromService(Service service)
        {
            return new ServiceViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                CategoryId = service.CategoryId,
                Price = service.Price,
                DurationMinutes = service.DurationMinutes,
                RepresentativeImage = service.RepresentativeImage,
                CreatedAt = service.CreatedAt,
                UpdatedAt = service.UpdatedAt
            };
        }
    }
}
