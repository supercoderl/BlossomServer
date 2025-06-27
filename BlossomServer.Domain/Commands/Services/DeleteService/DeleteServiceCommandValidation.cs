using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Services.DeleteService
{
    public sealed class DeleteServiceCommandValidation : AbstractValidator<DeleteServiceCommand>
    {
        public DeleteServiceCommandValidation()
        {
            
        }
    }
}
