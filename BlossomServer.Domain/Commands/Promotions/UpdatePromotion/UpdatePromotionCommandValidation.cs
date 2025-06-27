using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Promotions.UpdatePromotion
{
    public sealed class UpdatePromotionCommandValidation : AbstractValidator<UpdatePromotionCommand>
    {
        public UpdatePromotionCommandValidation()
        {
            
        }
    }
}
