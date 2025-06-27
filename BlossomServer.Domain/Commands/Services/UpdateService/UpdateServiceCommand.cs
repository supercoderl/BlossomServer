using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Services.UpdateService
{
    public sealed class UpdateServiceCommand : CommandBase, IRequest
    { 
        private static readonly UpdateServiceCommandValidation s_validation = new();

        public Guid ServiceId { get; }
        public string Name { get; }
        public string? Description { get; }
        public Guid CategoryId { get; }
        public decimal Price { get; }
        public int DurationMinutes { get; }
        public IFormFile? RepresentativeImage { get; }

        public UpdateServiceCommand(
            Guid serviceId,
            string name,
            string? description,
            Guid categoryId,
            decimal price,
            int durationMinutes,
            IFormFile? representativeImage
        ) : base(Guid.NewGuid())
        {
            ServiceId = serviceId;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            DurationMinutes = durationMinutes;
            RepresentativeImage = representativeImage;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
