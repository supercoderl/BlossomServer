using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Services.UpdateService
{
    public sealed class UpdateServiceCommandValidation : AbstractValidator<UpdateServiceCommand>
    {
        public UpdateServiceCommandValidation()
        {
            
        }
    }
}
