using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Categories
{
    public sealed record UpdateCategoryViewModel
    (
        Guid CategoryId,
        string Name,
        bool IsActive,
        string Icon,
        string Url,
        int Priority
    );
}
