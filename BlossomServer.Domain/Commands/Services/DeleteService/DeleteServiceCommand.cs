using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Services.DeleteService
{
    public sealed class DeleteServiceCommand : CommandBase, IRequest
    {
        private static readonly DeleteServiceCommandValidation s_validation = new();

        public Guid ServiceId { get; }

        public DeleteServiceCommand(Guid serviceId) : base(Guid.NewGuid())
        {
            ServiceId = serviceId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
