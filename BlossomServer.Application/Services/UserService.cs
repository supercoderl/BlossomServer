using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Users.GetAll;
using BlossomServer.Application.Queries.Users.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Commands.Users.ChangePassword;
using BlossomServer.Domain.Commands.Users.CreateUser;
using BlossomServer.Domain.Commands.Users.DeleteUser;
using BlossomServer.Domain.Commands.Users.ForgotPassword;
using BlossomServer.Domain.Commands.Users.GenerateGuestToken;
using BlossomServer.Domain.Commands.Users.Login;
using BlossomServer.Domain.Commands.Users.RefreshToken;
using BlossomServer.Domain.Commands.Users.ResetPassword;
using BlossomServer.Domain.Commands.Users.Revoke;
using BlossomServer.Domain.Commands.Users.SocialLogin;
using BlossomServer.Domain.Commands.Users.UpdateUser;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces;
using System.Text.Json;

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
            UserRole? role,
            bool includeDeleted,
            string searchTerm = "",
            bool excludeBot = true,
            SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllUsersQuery(query, role, includeDeleted, searchTerm, excludeBot, sortQuery));
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
                null,
                user.Gender,
                user.Website,
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
                user.CoverPhoto,
                user.Gender,
                user.Website,
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

        public async Task<LoginResponse> LoginUserAsync(LoginUserViewModel viewModel)
        {
            object result = await _bus.QueryAsync(new LoginUserCommand(viewModel.Identifier, viewModel.Password));
            return MapToLoginResponse(result);
        }

        public async Task<object> RefreshTokenAsync(RefreshTokenViewModel viewModel)
        {
            return await _bus.QueryAsync(new RefreshTokenCommand(viewModel.RefreshToken));
        }

        public async Task<Guid> ForgotPasswordAsync(ForgotPasswordViewModel viewModel)
        {
            var id = Guid.NewGuid();

            await _bus.SendCommandAsync(new ForgotPasswordCommand(id, viewModel.Identifier));

            return id;
        }

        public async Task ResetPasswordAsync(ResetPasswordViewModel viewModel)
        {
            await _bus.SendCommandAsync(new ResetPasswordCommand(viewModel.Code, viewModel.NewPassword));
        }

        public async Task<string> GenerateGuestTokenAsync(string clientId, string userAgent)
        {
            return await _bus.QueryAsync(new GenerateGuestTokenCommand(clientId, userAgent));
        }

        public async Task<LoginResponse> SocialLoginAsync(string provider, string code)
        {
            object result = await _bus.QueryAsync(new SocialLoginCommand(code, provider));
            return MapToLoginResponse(result);
        }

        private LoginResponse MapToLoginResponse(object result)
        {
            if (result == null)
            {
                return LoginResponse.Empty();
            }

            try
            {
                // Convert to JSON string then deserialize to our application layer DTO
                var json = JsonSerializer.Serialize(result);
                var loginData = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Check if login was successful (has AccessToken)
                if (string.IsNullOrEmpty(loginData?.AccessToken.Value))
                {
                    return LoginResponse.Empty();
                }

                return new LoginResponse
                {
                    AccessToken = loginData.AccessToken,
                    RefreshToken = loginData.RefreshToken
                };
            }
            catch
            {
                return LoginResponse.Empty();
            }
        }

        public async Task LogoutAsync(string refreshToken)
        {
            await _bus.SendCommandAsync(new RevokeCommand(refreshToken));
        }
    }
}
