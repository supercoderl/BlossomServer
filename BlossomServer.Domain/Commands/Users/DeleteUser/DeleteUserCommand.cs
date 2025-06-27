using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.DeleteUser
{
    public sealed class DeleteUserCommand : CommandBase
    {
        private static readonly DeleteUserCommandValidation s_validation = new();

        public Guid UserId { get; }

        public DeleteUserCommand(Guid userId) : base(userId)
        {
            UserId = userId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
