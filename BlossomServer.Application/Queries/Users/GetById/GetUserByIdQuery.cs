using BlossomServer.Application.ViewModels.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Queries.Users.GetById
{
    public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserViewModel?>;
}
