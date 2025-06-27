using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Users
{
    public sealed record LoginUserViewModel(string Identifier, string Password);
}
