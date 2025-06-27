using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Promotions.DeletePromotion
{
    public sealed class DeletePromotionCommand : CommandBase, IRequest
    {
        private static readonly DeletePromotionCommandValidation s_validation = new();

        public Guid PromotionId { get; }

        public DeletePromotionCommand(Guid promotionId) : base(Guid.NewGuid())
        {
            PromotionId = promotionId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
