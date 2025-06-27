using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Categories
{
    public sealed record CategoryViewModel
    (
        Guid Id,
        string Name,
        bool IsActive,
        int Priority
    );
}
