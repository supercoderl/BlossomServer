using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Services
{
    public sealed record UpdateServiceViewModel
    (
        Guid ServiceId,
        string Name,
        string? Description,
        Guid CategoryId,
        decimal Price,
        int DurationInMinutes,
        IFormFile? RepesentativeImage
    );
}
