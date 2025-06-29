using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class ServiceOption : Entity<Guid>
    {
        public Guid ServiceId { get; private set; }
        public string VariantName { get; private set; }
        public decimal PriceFrom { get; private set; }
        public decimal? PriceTo { get; private set; }   
        public int? DurationMinutes { get; private set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("ServiceOptions")]
        public virtual Service? Service { get; set; }

        [InverseProperty("ServiceOption")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

        public ServiceOption(
            Guid id,
            Guid serviceId,
            string variantName,
            decimal priceFrom,
            decimal? priceTo,
            int? durationMinutes
        ) : base(id)
        {
            ServiceId = serviceId;
            VariantName = variantName;
            PriceFrom = priceFrom;
            PriceTo = priceTo;
            DurationMinutes = durationMinutes;
        }

        public void SetVariantName(string variantName ) { VariantName = variantName; }
        public void SetPriceFrom(decimal priceFrom) { PriceFrom = priceFrom; }
        public void SetPriceTo(decimal? priceTo) { PriceTo = priceTo; }
        public void SetDurationMinutes(int? durationMinutes) { DurationMinutes = durationMinutes; }
    }
}
