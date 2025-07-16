using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BlossomServer.Application.Extensions;

namespace BlossomServer.Application.Queries.Users.GetAll
{
    public sealed class GetAllUsersQueryHandler :
        IRequestHandler<GetAllUsersQuery, PagedResult<UserViewModel>>
    {
        private readonly ISortingExpressionProvider<UserViewModel, User> _sortingExpressionProvider;
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(
            IUserRepository userRepository,
            ISortingExpressionProvider<UserViewModel, User> sortingExpressionProvider)
        {
            _userRepository = userRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<UserViewModel>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var usersQuery = _userRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Include(x => x.Technician)
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                usersQuery = usersQuery.Where(user =>
                    EF.Functions.Like(user.Email, $"%{request.SearchTerm}%") ||
                    EF.Functions.Like(user.FirstName, $"%{request.SearchTerm}%") ||
                    EF.Functions.Like(user.LastName, $"%{request.SearchTerm}%"));
            }

            var totalCount = await usersQuery.CountAsync(cancellationToken);

            usersQuery = usersQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var users = await usersQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(user => UserViewModel.FromUser(user, "web", null, null))
                .ToListAsync(cancellationToken);

            return new PagedResult<UserViewModel>(
                totalCount, users, request.Query.Page, request.Query.PageSize);
        }
    }
}
