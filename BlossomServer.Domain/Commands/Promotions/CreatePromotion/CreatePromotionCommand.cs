using BlossomServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Promotions.CreatePromotion
{
    public sealed class CreatePromotionCommand : CommandBase, IRequest
    {
        private static readonly CreatePromotionCommandValidation s_validation = new();

        public Guid PromotionId { get; }
        public string Code { get; }
        public string? Description { get; }
        public DiscountType DiscountType { get; }
        public decimal DiscountValue { get; }
        public decimal MinimumSpend { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int MaxUsage { get; }
        public int CurrentUsage { get; }
        public bool IsActive { get; }

        public CreatePromotionCommand(
            Guid promotionId,
            string code,
            string? description,
            DiscountType discountType,
            decimal discountValue,
            decimal minimumSpend,
            DateTime startDate,
            DateTime endDate,
            int maxUsage,
            int currentUsage,
            bool isActive
        ) : base(Guid.NewGuid())
        {
            PromotionId = promotionId;
            Code = code;
            Description = description;
            DiscountType = discountType;
            DiscountValue = discountValue;
            MinimumSpend = minimumSpend;
            StartDate = startDate;
            EndDate = endDate;
            MaxUsage = maxUsage;
            CurrentUsage = currentUsage;
            IsActive = isActive;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
