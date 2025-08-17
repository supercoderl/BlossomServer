using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Enums;

namespace BlossomServer.Application.Queries.Users.GetAll
{
    public sealed record GetAllUsersQuery(
        PageQuery Query,
        UserRole? Role,
        bool IncludeDeleted,
        string SearchTerm = "",
        bool ExcludeBot = true,
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<UserViewModel>>;
}
