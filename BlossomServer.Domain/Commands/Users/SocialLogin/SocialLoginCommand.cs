using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.SocialLogin
{
    public sealed class SocialLoginCommand : CommandBase, IRequest<object>
    {
        private static readonly SocialLoginCommandValidation s_validation = new();

        public string Code { get; }
        public string Provider { get; }

        public SocialLoginCommand(
            string code,
            string provider
        ) : base(Guid.NewGuid())
        {
            Code = code;
            Provider = provider;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
