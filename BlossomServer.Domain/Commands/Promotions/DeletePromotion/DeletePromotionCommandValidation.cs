using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Promotions.DeletePromotion
{
    public sealed class DeletePromotionCommandValidation : AbstractValidator<DeletePromotionCommand>
    {
        public DeletePromotionCommandValidation()
        {
            
        }
    }
}
