using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.UpdateServiceOption
{
    public sealed class UpdateServiceOptionCommand : CommandBase, IRequest
    {
        private static readonly UpdateServiceOptionCommandValidation s_validation = new();

        public Guid ServiceOptionId { get; }
        public string VariantName { get; }
        public decimal PriceFrom { get; }
        public decimal? PriceTo { get; }
        public int? DurationMinutes { get; }

        public UpdateServiceOptionCommand(
            Guid serviceOptionId,
            string variantName,
            decimal priceFrom,
            decimal? priceTo,
            int? durationMinutes
        ) : base(Guid.NewGuid())
        {
            ServiceOptionId = serviceOptionId;
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
