using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Extensions.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.Login
{
    public sealed class LoginUserCommandValidation : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidation()
        {
            AddRuleForEmail();
            AddRuleForPassword();
        }

        private void AddRuleForEmail()
        {
            RuleFor(cmd => cmd.Identifier)
                .NotEmpty().WithMessage("Identifier is required")
                .Must(BeAValidEmailOrUsername)
                .WithMessage("Identifier must be a valid email address or a valid username")
                .WithErrorCode(DomainErrorCodes.User.InvalidIdentifier);
        }

        private void AddRuleForPassword()
        {
            RuleFor(cmd => cmd.Password)
                .Password();
        }

        private bool BeAValidEmailOrUsername(string identifier)
        {
            // Check if it's a valid email
            var isEmail = Regex.IsMatch(identifier,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

            // Or a valid username (alphanumeric, no spaces, length >= 3 for example)
            var isUsername = Regex.IsMatch(identifier,
                @"^[a-zA-Z0-9_.-]{3,}$");

            return isEmail || isUsername;
        }
    }
}
