using BlossomServer.Application.Interfaces;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Users;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using BlossomServer.SharedKernel.Utils;
using BlossomServer.Swagger;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/v1/[controller]")]
    public sealed class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly OAuthHelper _oAuthHelper;
        private readonly IConfiguration _configuration;

        public UserController(
            INotificationHandler<DomainNotification> notifications,
            IUserService userService,
            OAuthHelper oAuthHelper,
            IConfiguration configuration
        ) : base(notifications)
        {
            _userService = userService;
            _oAuthHelper = oAuthHelper;
            _configuration = configuration;
        }

        [HttpGet]
        [SwaggerOperation("Get a list of all users")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<UserViewModel>>))]
        public async Task<IActionResult> GetAllUsersAsync(
            [FromQuery] PageQuery query,
            [FromQuery] UserRole? role,
            [FromQuery] string searchTerm = "",
            [FromQuery] bool includeDeleted = false,
            [FromQuery] bool excludeBot = true,
            [FromQuery] [SortableFieldsAttribute<UserViewModelSortProvider, UserViewModel, User>]
        SortQuery? sortQuery = null)
        {
            var users = await _userService.GetAllUsersAsync(
                query,
                role,
                includeDeleted,
                searchTerm,
                excludeBot,
                sortQuery);
            return Response(users);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get a user by id")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UserViewModel>))]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByUserIdAsync(id);
            return Response(user);
        }

        [HttpGet("me")]
        [SwaggerOperation("Get the current active user")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UserViewModel>))]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            return Response(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Create a new user")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserViewModel viewModel)
        {
            var userId = await _userService.CreateUserAsync(viewModel);
            return Response(userId);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete a user")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Response(id);
        }

        [HttpPut]
        [SwaggerOperation("Update a user")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<UpdateUserViewModel>))]
        public async Task<IActionResult> UpdateUserAsync([FromForm] UpdateUserViewModel viewModel)
        {
            await _userService.UpdateUserAsync(viewModel);
            return Response(viewModel);
        }

        [HttpPost("changePassword")]
        [SwaggerOperation("Change a password for the current active user")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<ChangePasswordViewModel>))]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordViewModel viewModel)
        {
            await _userService.ChangePasswordAsync(viewModel);
            return Response(viewModel);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation("Get a signed token for a user")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<object>))]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserViewModel viewModel)
        {
            var token = await _userService.LoginUserAsync(viewModel);
            // Set new cookies
            SetTokenCookie("access_token", token.AccessToken);
            SetTokenCookie("refresh_token", token.RefreshToken);

            return Response("Login successful");
        }

        [HttpPost("rt/refresh-token")]
        [AllowAnonymous]
        [SwaggerOperation("Get a signed token for a user")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<object>))]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenViewModel viewModel)
        {
            var token = await _userService.RefreshTokenAsync(viewModel);
            return Response(token);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [SwaggerOperation("Send token for users to reset their password")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordViewModel viewModel)
        {
            var id = await _userService.ForgotPasswordAsync(viewModel);
            return Response(id);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerOperation("Reset password's user")]
        [SwaggerResponse(200, "Request successful")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordViewModel viewModel)
        {
            await _userService.ResetPasswordAsync(viewModel);
            return Response();
        }

        [HttpPost("social-login")]
        [AllowAnonymous]
        [SwaggerOperation("Login user with social network")]
        [SwaggerResponse(200, "Request successful")]
        public IActionResult SocialLoginAsync([FromQuery] string provider)
        {
            var state = provider;
            var authUrl = _oAuthHelper.GenerateAuthUrl(provider, state);
            return Response(authUrl);
        }

        [HttpGet("csrf-token")]
        [AllowAnonymous]
        public IActionResult GetCsrfToken()
        {
            var tokens = HttpContext.RequestServices.GetRequiredService<IAntiforgery>();
            var token = tokens.GetAndStoreTokens(HttpContext);

            return Response(token);
        }

        [HttpGet("callback")]
        [AllowAnonymous]
        [SwaggerOperation("Callback to check token")]
        [SwaggerResponse(200, "Request successful")]
        public async Task<IActionResult> CallbackAsync([FromQuery] string state, [FromQuery] string code)
        {
            var token = await _userService.SocialLoginAsync(state, code);
            var clientUrl = _configuration["Client:BaseUrl"];

            // Set new cookies
            SetTokenCookie("access_token", token.AccessToken);
            SetTokenCookie("refresh_token", token.RefreshToken);

            return Redirect(string.Concat(clientUrl, "/dashboard"));
        }

        [HttpPost("rt/logout")]
        [SwaggerOperation("Logout user")]
        [SwaggerResponse(200, "Request successful")]
        public async Task<IActionResult> LogoutAsync()
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var rt))
            {
                await _userService.LogoutAsync(rt); 
            }

            // Clear cookies
            RemoveTokenCookie("access_token");
            RemoveTokenCookie("refresh_token");

            return Response();
        }
        private void SetTokenCookie(string key, Token token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = key == "access_token" ? TimeZoneHelper.GetLocalTimeNow().AddMinutes(token.Exp) : TimeZoneHelper.GetLocalTimeNow().AddDays(token.Exp), // Short expiration for token
                Path = key == "access_token" ? "/" : "/api/v1/User/rt",
                Domain = null
            };

            HttpContext.Response.Cookies.Append(key, token.Value, cookieOptions);
        }

        private void RemoveTokenCookie(string key)
        {
            HttpContext.Response.Cookies.Delete(key, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = key == "access_token" ? "/" : "/api/v1/User/rt"
            });
        }
    }
}
