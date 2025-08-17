using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Contacts
{
    public sealed record CreateContactViewModel
    (
        string Name,
        string Email,
        string Message
    );
}
