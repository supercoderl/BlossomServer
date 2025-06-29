using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.CreateServiceOption
{
    public sealed class CreateServiceOptionCommand : CommandBase, IRequest
    {
        private static readonly CreateServiceOptionCommandValidation s_validation = new();

        public Guid ServiceOptionId { get; }
        public Guid ServiceId { get; }
        public string VariantName { get; }
        public decimal PriceFrom { get; }
        public decimal? PriceTo { get; }
        public int? DurationMinutes { get; }

        public CreateServiceOptionCommand(
            Guid serviceOptionId,
            Guid serviceId,
            string variantName,
            decimal priceFrom,
            decimal? priceTo,
            int? durationMinutes
        ) : base(Guid.NewGuid())
        {
            ServiceOptionId = serviceOptionId;
            ServiceId = serviceId;
            VariantName = variantName;
            PriceFrom = priceFrom;
            PriceTo = priceTo;
            DurationMinutes = durationMinutes;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
