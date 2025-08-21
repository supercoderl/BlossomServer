using MediatR;

namespace BlossomServer.Domain.Commands.Users.GenerateGuestToken
{
    public sealed class GenerateGuestTokenCommand : CommandBase, IRequest<string>
    {
        private static readonly GenerateGuestTokenCommandValidation s_validation = new();

        public string ClientId { get; }
        public string UserAgent { get; }

        public GenerateGuestTokenCommand(
            string clientId,
            string userAgent
        ) : base(Guid.NewGuid())
        {
            ClientId = clientId;
            UserAgent = userAgent;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
