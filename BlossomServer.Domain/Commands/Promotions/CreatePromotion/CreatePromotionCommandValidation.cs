using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Promotions.CreatePromotion
{
    public sealed class CreatePromotionCommandValidation : AbstractValidator<CreatePromotionCommand>
    {
        public CreatePromotionCommandValidation()
        {
            
        }
    }
}
