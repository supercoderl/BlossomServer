using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.ServiceOptions
{
    public sealed class ServiceOptionViewModel
    {
        public Guid ServiceOptionId { get; set; }
        public Guid ServiceId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public decimal PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public int? DurationMinutes { get; set; }

        public static ServiceOptionViewModel FromServiceOption(ServiceOption serviceOption)
        {
            return new ServiceOptionViewModel
            {
                ServiceOptionId = serviceOption.Id,
                ServiceId = serviceOption.ServiceId,
                VariantName = serviceOption.VariantName,
                PriceFrom = serviceOption.PriceFrom,
                PriceTo = serviceOption.PriceTo,
                DurationMinutes = serviceOption.DurationMinutes
            };
        }
    }
}
