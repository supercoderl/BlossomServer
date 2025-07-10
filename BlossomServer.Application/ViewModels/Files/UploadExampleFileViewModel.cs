using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Files
{
    public sealed record UploadExampleFileViewModel
    (
        IFormFile File
    );
}
