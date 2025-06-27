using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Promotions
{
    public sealed record UpdatePromotionViewModel
    (
        Guid PromotionId,
        string Code,
        string? Description,
        DiscountType DiscountType,
        decimal DiscountValue,
        decimal MinimumSpend,
        DateTime StartDate,
        DateTime EndDate,
        int CurrentUsage,
        int MaxUsage,
        bool IsActive
    );
}
