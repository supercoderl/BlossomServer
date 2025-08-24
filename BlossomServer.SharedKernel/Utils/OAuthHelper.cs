using BlossomServer.SharedKernel.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace BlossomServer.SharedKernel.Utils
{
    public class OAuthHelper
    {
        private readonly Dictionary<string, OAuthProviderOptions> _providers;

        public OAuthHelper(
            IOptions<Dictionary<string,
                OAuthProviderOptions>> providers
        )
        {
            _providers = providers.Value;
        }

        public string GenerateAuthUrl(string provider, string state)
        {
            if (!_providers.TryGetValue(provider, out var options))
                throw new ArgumentException("Provider not supported");

            return $"{options.AuthorizationEndpoint}" +
                   $"?client_id={options.ClientId}" +
                   $"&redirect_uri={Uri.EscapeDataString(options.RedirectUri)}" +
                   $"&response_type=code" +
                   $"&scope={Uri.EscapeDataString(options.Scope)}" +
                   $"&state={Uri.EscapeDataString(state)}";
        }

        public static string GenerateSecureRandomPassword()
        {
            // Generate a more secure random password instead of hardcoded value
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static object DecodeIdToken(string idToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(idToken);

            var payload = new
            {
                Email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                EmailVerified = bool.Parse(jwt.Claims.FirstOrDefault(c => c.Type == "email_verified")?.Value ?? "false"),
                Name = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Picture = jwt.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
            };

            return payload;
        }

        public static string ExtractGoogleToken(string json)
        {
            using var doc = JsonDocument.Parse(json);

            // For Google, we can use either access_token or id_token
            // id_token contains user info directly (JWT), access_token requires additional API call
            if (doc.RootElement.TryGetProperty("id_token", out var idToken))
            {
                return idToken.GetString() ?? string.Empty;
            }

            if (doc.RootElement.TryGetProperty("access_token", out var accessToken))
            {
                return accessToken.GetString() ?? string.Empty;
            }

            throw new Exception("Could not extract token from Google response");
        }

        public static string ExtractGitHubToken(string responseContent)
        {
            // GitHub can return either JSON or form-encoded response
            if (responseContent.StartsWith("{"))
            {
                using var doc = JsonDocument.Parse(responseContent);
                if (doc.RootElement.TryGetProperty("access_token", out var token))
                {
                    return token.GetString() ?? string.Empty;
                }
            }
            else
            {
                // Parse form-encoded response: access_token=...&scope=...&token_type=bearer
                var pairs = responseContent.Split('&');
                var tokenPair = pairs.FirstOrDefault(p => p.StartsWith("access_token="));
                if (tokenPair != null)
                {
                    return tokenPair.Split('=')[1];
                }
            }

            throw new Exception("Could not extract token from GitHub response");
        }

        public static string ExtractFacebookToken(string json)
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("access_token", out var token))
            {
                return token.GetString() ?? string.Empty;
            }

            throw new Exception("Could not extract token from Facebook response");
        }
    }
}
