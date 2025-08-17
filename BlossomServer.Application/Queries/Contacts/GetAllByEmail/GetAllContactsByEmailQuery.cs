using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Queries.Contacts.GetAllByEmail
{
    public sealed record GetAllContactsByEmailQuery(
        PageQuery Query,
        bool IncludeResponses,
        string Email,
        SortQuery? SortQuery = null
    ) :
        IRequest<PagedResult<ContactViewModel>>;
}
