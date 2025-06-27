using MediatR;

namespace BlossomServer.Domain.Commands.Users.Login
{
    public sealed class LoginUserCommand : CommandBase,
        IRequest<object>
    {
        private static readonly LoginUserCommandValidation s_validation = new();

        public string Identifier { get; set; }
        public string Password { get; set; }


        public LoginUserCommand(
            string identifier,
            string password) : base(Guid.NewGuid())
        {
            Identifier = identifier;
            Password = password;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
