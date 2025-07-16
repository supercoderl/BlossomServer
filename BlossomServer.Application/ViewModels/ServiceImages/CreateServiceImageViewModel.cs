using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.ServiceImages
{
    public sealed record CreateServiceImageViewModel
    (
        List<IFormFile> ImageFile,
        Guid ServiceId,
        string? Description
    );
}
