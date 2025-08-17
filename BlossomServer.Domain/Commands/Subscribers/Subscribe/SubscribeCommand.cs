using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Subscribers.Subscribe
{
    public sealed class SubscribeCommand : CommandBase, IRequest
    {
        private static readonly SubscribeCommandValidation s_validation = new();

        public Guid SubscriberId { get; }
        public string Email { get; }

        public SubscribeCommand(
            Guid subscriberId,
            string email
        ) : base(Guid.NewGuid())
        {
            SubscriberId = subscriberId;
            Email = email;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
