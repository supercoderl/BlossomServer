using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.SortProviders
{
    public sealed class UserViewModelSortProvider : ISortingExpressionProvider<UserViewModel, User>
    {
        private static readonly Dictionary<string, Expression<Func<User, object>>> s_expressions = new()
    {
        { "email", user => user.Email },
        { "firstName", user => user.FirstName },
        { "lastName", user => user.LastName },
        { "lastloggedindate", user => user.LastLoggedinDate ?? DateTimeOffset.MinValue },
        { "phoneNumber", user => user.PhoneNumber },
        { "status", user => user.Status }
    };

        public Dictionary<string, Expression<Func<User, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
