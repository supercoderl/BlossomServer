using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Helpers;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using BlossomServer.SharedKernel.Models;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace BlossomServer.Domain.Commands.Users.SocialLogin
{
    public sealed class SocialLoginCommandHandler : CommandHandlerBase, IRequestHandler<SocialLoginCommand, object>
    {
        private readonly IUserRepository _userRepository;
        private readonly Dictionary<string, OAuthProviderOptions> _providers;
        private readonly TokenSettings _tokenSettings;
        private readonly HttpClient _httpClient;

        public SocialLoginCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository,
            IOptions<TokenSettings> tokenOption,
            IOptions<Dictionary<string, OAuthProviderOptions>> providers,
            IHttpClientFactory httpClientFactory
        ) : base(bus, unitOfWork, notifications)
        {
            _userRepository = userRepository;
            _providers = providers.Value;
            _tokenSettings = tokenOption.Value;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<object> Handle(SocialLoginCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request))
                return new {};

            if (!_providers.TryGetValue(request.Provider, out var options))
                throw new ArgumentException($"Provider '{request.Provider}' not supported", nameof(request.Provider));

            // Exchange code for token
            var idToken = await ExchangeCodeForToken(request.Code, options);

            if (string.IsNullOrEmpty(idToken))
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"Cannot login with credential for provider: {request.Provider}",
                    ErrorCodes.InsufficientPermissions
                ));
                return new { };
            }

            dynamic tokenDecoded = options.Provider == "Google" ? OAuthHelper.DecodeIdToken(idToken) : await GetGitHubUserInfo(idToken);

            if (!IsValidDecodedToken(tokenDecoded))
                return new {};

            var user = await GetOrCreateUserAsync(tokenDecoded, cancellationToken);
            if (user == null)
                return new {};

            // Generate tokens and return response
            return await GenerateLoginResponseAsync(user, cancellationToken);
        }

        private async Task<object> GenerateLoginResponseAsync(Entities.User user, CancellationToken cancellationToken)
        {
            var refreshToken = await TokenHelper.GenerateRefreshToken(user.Id, Bus, _tokenSettings);

            if (!await CommitAsync())
                return new {};

            return new
            {
                AccessToken = new
                {
                    Value = TokenHelper.BuildToken(user, _tokenSettings),
                    Exp = _tokenSettings.ExpiryDurationMinutes
                },
                RefreshToken = new
                {
                    Value = refreshToken,
                    Exp = _tokenSettings.ExpiryDurationDays
                }
            };
        }

        private static bool IsValidDecodedToken(dynamic tokenDecoded)
        {
            return tokenDecoded?.EmailVerified == true && !string.IsNullOrEmpty(tokenDecoded?.Email);
        }

        private async Task<string> ExchangeCodeForToken(string code, OAuthProviderOptions options)
        {
            var formData = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", options.ClientId },
                { "client_secret", options.ClientSecret }
            };

            // Add grant_type based on provider
            if (Enum.TryParse<OAuthProvider>(options.Provider, true, out var provider))
            {
                switch (provider)
                {
                    case OAuthProvider.Google:
                    case OAuthProvider.Facebook:
                        formData.Add("grant_type", "authorization_code");
                        break;
                    case OAuthProvider.GitHub:
                        // GitHub doesn't require grant_type in the request
                        break;
                    default:
                        throw new NotSupportedException($"Provider {options.Provider} is not supported");
                }

                var response = await _httpClient.PostAsync(options.TokenEndpoint, new FormUrlEncodedContent(formData));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                return provider switch
                {
                    OAuthProvider.Google => OAuthHelper.ExtractGoogleToken(json),
                    OAuthProvider.GitHub => OAuthHelper.ExtractGitHubToken(json),
                    OAuthProvider.Facebook => OAuthHelper.ExtractFacebookToken(json),
                    _ => throw new NotSupportedException($"Provider {options.Provider} is not supported")
                };
            }
            else
            {
                throw new ArgumentException($"Invalid provider: {options.Provider}");
            }
        }

        private async Task<object> GetGitHubUserInfo(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Blossom Nails");
            var response = await _httpClient.GetAsync("https://api.github.com/user");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var email = await GetGitHubUserEmail(accessToken);

                return new
                {
                    Name = root.GetProperty("name").GetString(),
                    Picture = root.GetProperty("avatar_url").GetString(),
                    Email = email ?? string.Empty,
                    EmailVerified = !string.IsNullOrEmpty(email),
                    Website = root.GetProperty("blog").GetString(),
                };
            }
            return new {};
        }

        private async Task<string?> GetGitHubUserEmail(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Blossom Nails");
            var response = await _httpClient.GetAsync("https://api.github.com/user/emails");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                // Get the primary email
                foreach (var email in doc.RootElement.EnumerateArray())
                {
                    if (email.TryGetProperty("primary", out var primary) && primary.GetBoolean())
                    {
                        return email.GetProperty("email").GetString();
                    }
                }
            }
            return null;
        }

/*        private async Task<string?> GetFacebookUserEmail(string accessToken, OAuthProviderOptions options)
        {
            var url = $"{options.UserInfoEndpoint}?fields=id,name,email&access_token={accessToken}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<FacebookUserInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new UserInfo
            {
                Email = userInfo?.email ?? string.Empty,
                Name = userInfo?.name ?? string.Empty,
                Id = userInfo?.id ?? string.Empty
            };
        }*/

        private async Task<Entities.User?> GetOrCreateUserAsync(dynamic tokenDecoded, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdentifierAsync(tokenDecoded.Email);

            if (user != null)
            {
                // Update last login for existing user
                user.SetLastLoggedIn(TimeZoneHelper.GetLocalTimeNow());
                return user;
            }

            // Create new user
            var newUser = CreateNewUser(tokenDecoded);
            _userRepository.Add(newUser);

            if (!await CommitAsync())
                return null;

            return newUser;
        }

        private static Entities.User CreateNewUser(dynamic tokenDecoded)
        {
            var newUser = new Entities.User(
                Guid.NewGuid(),
                BCrypt.Net.BCrypt.HashPassword(OAuthHelper.GenerateSecureRandomPassword()),
                tokenDecoded.Name ?? string.Empty,
                string.Empty, // last name
                tokenDecoded.Email,
                string.Empty, // phone number
                tokenDecoded.Picture ?? string.Empty,
                null, // cover photo
                Enums.Gender.Unknow,
                tokenDecoded.Website, // website
                DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                Enums.UserRole.Customer,
                Enums.UserStatus.Active
            );

            newUser.SetLastLoggedIn(TimeZoneHelper.GetLocalTimeNow());
            return newUser;
        }
    }
}
