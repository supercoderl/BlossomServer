using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Application.ViewModels.Users;

namespace BlossomServer.Application.Queries.Users.GetAll
{
    public sealed record GetAllUsersQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<UserViewModel>>;
}
