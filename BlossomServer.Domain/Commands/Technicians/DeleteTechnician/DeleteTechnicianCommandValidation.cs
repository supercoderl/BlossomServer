using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Technicians.DeleteTechnician
{
    public sealed class DeleteTechnicianCommandValidation : AbstractValidator<DeleteTechnicianCommand>
    {
        public DeleteTechnicianCommandValidation()
        {

        }
    }
}
