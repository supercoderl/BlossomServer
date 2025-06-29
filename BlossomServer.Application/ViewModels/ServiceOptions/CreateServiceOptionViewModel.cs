using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.ServiceOptions
{
    public sealed record CreateServiceOptionViewModel
    (
        Guid ServiceId,
        string VariantName,
        decimal PriceFrom,
        decimal? PriceTo,
        int? DurationMinutes
    );
}
