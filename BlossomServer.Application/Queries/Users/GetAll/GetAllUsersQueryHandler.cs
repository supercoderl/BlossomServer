using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

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
            var results = await _userRepository.GetAllUsersBySQL(
                request.SearchTerm,
                request.Role,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.ExcludeBot,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var users = results.Select(u => UserViewModel.FromUser(u, null, null, null)).ToList();

            return new PagedResult<UserViewModel>(results.Count(), users, request.Query.Page, request.Query.PageSize);
        }
    }
}
