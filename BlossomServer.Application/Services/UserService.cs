using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Users.GetAll;
using BlossomServer.Application.Queries.Users.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Commands.Users.ChangePassword;
using BlossomServer.Domain.Commands.Users.CreateUser;
using BlossomServer.Domain.Commands.Users.DeleteUser;
using BlossomServer.Domain.Commands.Users.Login;
using BlossomServer.Domain.Commands.Users.RefreshToken;
using BlossomServer.Domain.Commands.Users.UpdateUser;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public UserService(IMediatorHandler bus, IUser user)
        {
            _bus = bus;
            _user = user;
        }

        public async Task<UserViewModel?> GetUserByUserIdAsync(Guid userId)
        {
            return await _bus.QueryAsync(new GetUserByIdQuery(userId));
        }

        public async Task<UserViewModel?> GetCurrentUserAsync()
        {
            return await _bus.QueryAsync(new GetUserByIdQuery(_user.GetUserId()));
        }

        public async Task<PagedResult<UserViewModel>> GetAllUsersAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllUsersQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<Guid> CreateUserAsync(CreateUserViewModel user)
        {
            var userId = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateUserCommand(
                userId,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Password,
                user.PhoneNumber,
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSvuBGQpwoDKnv2asTcJdE7YqEtovBNUUL2hQ&s",
                user.Gender,
                user.DateOfBirth,
                user.Role
            ));

            return userId;
        }

        public async Task UpdateUserAsync(UpdateUserViewModel user)
        {
            await _bus.SendCommandAsync(new UpdateUserCommand(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.AvatarFile,
                user.Gender,
                user.DateOfBirth,
                user.Role
            ));
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            await _bus.SendCommandAsync(new DeleteUserCommand(userId));
        }

        public async Task ChangePasswordAsync(ChangePasswordViewModel viewModel)
        {
            await _bus.SendCommandAsync(new ChangePasswordCommand(viewModel.OldPassword, viewModel.NewPassword));
        }

        public async Task<object> LoginUserAsync(LoginUserViewModel viewModel)
        {
            return await _bus.QueryAsync(new LoginUserCommand(viewModel.Identifier, viewModel.Password));
        }

        public async Task<object> RefreshTokenAsync(RefreshTokenViewModel viewModel)
        {
            return await _bus.QueryAsync(new RefreshTokenCommand(viewModel.RefreshToken));
        }
    }
}
