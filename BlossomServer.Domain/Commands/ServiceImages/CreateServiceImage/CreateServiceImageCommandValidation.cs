using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.CreateServiceImage
{
    public sealed class CreateServiceImageCommandValidation : AbstractValidator<CreateServiceImageCommand>
    {
        public CreateServiceImageCommandValidation()
        {
            
        }
    }
}
