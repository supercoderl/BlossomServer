using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Categories
{
    public sealed record CreateCategoryViewModel
    (
        string Name,
        int Priority,
        string Icon,
        string Url
    );
}
