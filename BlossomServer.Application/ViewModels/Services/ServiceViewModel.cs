using BlossomServer.Application.ViewModels.ServiceOptions;
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
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public int? DurationMinutes { get; set; }
        public string? RepresentativeImage { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<ServiceOptionViewModel> Options { get; set; } = new List<ServiceOptionViewModel>();

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
                UpdatedAt = service.UpdatedAt,
                Options = service.ServiceOptions.Select(so => ServiceOptionViewModel.FromServiceOption(so))
            };
        }
    }
}
