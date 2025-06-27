using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Technicians
{
    public sealed record UpdateTechnicianViewModel
    (
        Guid TechnicianId,
        string Bio,
        double Rating,
        int YearsOfExperience
    );
}
