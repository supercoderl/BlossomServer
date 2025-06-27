using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Technicians.CreateTechnician
{
    public sealed class CreateTechnicianCommandValidation : AbstractValidator<CreateTechnicianCommand>
    {
        public CreateTechnicianCommandValidation()
        {
            
        }
    }
}
