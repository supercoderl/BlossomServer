using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Technicians.UpdateTechnician
{
    public sealed class UpdateTechnicianCommandValidation : AbstractValidator<UpdateTechnicianCommand>
    {
        public UpdateTechnicianCommandValidation()
        {
            
        }
    }
}
