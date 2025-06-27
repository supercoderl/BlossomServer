using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Services.CreateService
{
    public sealed class CreateServiceCommandValidation : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceCommandValidation()
        {
            
        }
    }
}
