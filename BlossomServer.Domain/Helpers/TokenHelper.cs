using BlossomServer.Domain.Commands.RefreshToken.CreateRefreshToken;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Settings;
using BlossomServer.SharedKernel.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlossomServer.Domain.Helpers
{
    public static class TokenHelper
    {
        private const double _expiryDurationMinutes = 60;
        private const double _expiryDurationDays = 45;

        public static string BuildToken(User user, TokenSettings tokenSettings)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenSettings.Secret));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new JwtSecurityToken(
                tokenSettings.Issuer,
                tokenSettings.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_expiryDurationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public static async Task<string> GenerateRefreshToken(Guid userId, IMediatorHandler bus)
        {
            var randomNumber = new Byte[32];
            var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomNumber);
            string token = Convert.ToBase64String(randomNumber);

            await bus.SendCommandAsync(new CreateRefreshTokenCommand(
               Guid.NewGuid(),
               userId,
               token,
               TimeZoneHelper.GetLocalTimeNow().AddDays(_expiryDurationDays)
            ));

            return token;
        }

        public static string Generate6DigitToken()
        {
            var rng = new Random();
            int token = rng.Next(100000, 999999);
            return token.ToString();
        }

        public static string GenerateResetPasswordToken(int byteLength = 32)
        {
            byte[] randomBytes = new byte[byteLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
        }
    }
}
