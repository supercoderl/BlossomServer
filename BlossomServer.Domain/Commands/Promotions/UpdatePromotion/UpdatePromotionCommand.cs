using BlossomServer.Domain.Commands.Promotions.CreatePromotion;
using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Promotions.UpdatePromotion
{
    public sealed class UpdatePromotionCommand : CommandBase, IRequest
    {
        private static readonly UpdatePromotionCommandValidation s_validation = new();

        public Guid PromotionId { get; }
        public string Code { get; }
        public string? Description { get; }
        public DiscountType DiscountType { get; }
        public decimal DiscountValue { get; }
        public decimal MinimumSpend { get; }
        public int CurrentUsage { get; set; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int MaxUsage { get; }
        public bool IsActive { get; }

        public UpdatePromotionCommand(
            Guid promotionId,
            string code,
            string? description,
            DiscountType discountType,
            decimal discountValue,
            decimal minimumSpend,
            int currentUsage,
            DateTime startDate,
            DateTime endDate,
            int maxUsage,
            bool isActive
        ) : base(Guid.NewGuid())
        {
            PromotionId = promotionId;
            Code = code;
            Description = description;
            DiscountType = discountType;
            DiscountValue = discountValue;
            MinimumSpend = minimumSpend;
            CurrentUsage = currentUsage;
            StartDate = startDate;
            EndDate = endDate;
            MaxUsage = maxUsage;
            IsActive = isActive;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
