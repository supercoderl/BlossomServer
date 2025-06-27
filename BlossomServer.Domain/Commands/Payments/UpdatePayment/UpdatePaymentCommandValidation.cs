using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Payments.UpdatePayment
{
    public sealed class UpdatePaymentCommandValidation : AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentCommandValidation()
        {
            
        }
    }
}
