using MediatR;

namespace BlossomServer.Domain.Commands.Users.Revoke
{
    public sealed class RevokeCommand : CommandBase, IRequest
    {
        private static readonly RevokeCommandValidation s_validation = new();

        public string RefreshToken { get; }

        public RevokeCommand(string refreshToken) : base(Guid.NewGuid())
        {
            RefreshToken = refreshToken;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
