using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.RefreshToken.CreateRefreshToken
{
    public sealed class CreateRefreshTokenCommand : CommandBase, IRequest
    {
        private static readonly CreateRefreshTokenCommandValidation s_validation = new();

        public Guid RefreshTokenId { get; }
        public Guid UserId { get; }
        public string Token { get; }
        public DateTime ExpiryDate { get; }

        public CreateRefreshTokenCommand(
            Guid refreshTokenId,
            Guid userId,
            string token,
            DateTime expiryDate
        ) : base(Guid.NewGuid())
        {
            RefreshTokenId = refreshTokenId;
            UserId = userId;
            Token = token;
            ExpiryDate = expiryDate;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
