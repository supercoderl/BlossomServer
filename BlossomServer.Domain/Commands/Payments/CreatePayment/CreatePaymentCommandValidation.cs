using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Payments.CreatePayment
{
    public sealed class CreatePaymentCommandValidation : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidation()
        {
            
        }
    }
}
