using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Enums;

namespace BlossomServer.Application.Interfaces
{
    public interface IUserService
    {
        public Task<UserViewModel?> GetUserByUserIdAsync(Guid userId);
        public Task<UserViewModel?> GetCurrentUserAsync();

        public Task<PagedResult<UserViewModel>> GetAllUsersAsync(
            PageQuery query,
            UserRole? role,
            bool includeDeleted,
            string searchTerm = "",
            bool excludeBot = true,
            SortQuery? sortQuery = null);

        public Task<Guid> CreateUserAsync(CreateUserViewModel user);
        public Task UpdateUserAsync(UpdateUserViewModel user);
        public Task DeleteUserAsync(Guid userId);
        public Task ChangePasswordAsync(ChangePasswordViewModel viewModel);
        public Task<object> LoginUserAsync(LoginUserViewModel viewModel);
        public Task<object> RefreshTokenAsync(RefreshTokenViewModel viewModel);
        public Task<Guid> ForgotPasswordAsync(ForgotPasswordViewModel viewModel);
        public Task ResetPasswordAsync(ResetPasswordViewModel viewModel);
        public Task<string> GenerateGuestTokenAsync(string clientId, string userAgent);
    }
}
