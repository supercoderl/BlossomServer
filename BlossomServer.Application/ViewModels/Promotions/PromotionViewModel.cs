using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Promotions
{
    public sealed class PromotionViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinimumSpend { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsage { get; set; }
        public int CurrentUsage { get; set; }
        public bool IsActive { get; set; }

        public static PromotionViewModel FromPromotion(Promotion promotion)
        {
            return new PromotionViewModel
            {
                Id = promotion.Id,
                Code = promotion.Code,
                Description = promotion.Description,
                DiscountType = promotion.DiscountType,
                DiscountValue = promotion.DiscountValue,
                MinimumSpend = promotion.MinimumSpend,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                MaxUsage = promotion.MaxUsage,
                CurrentUsage = promotion.CurrentUsage,
                IsActive = promotion.IsActive
            };
        }
    }
}
