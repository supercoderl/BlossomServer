using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.DeleteServiceOption
{
    public sealed class DeleteServiceOptionCommand : CommandBase, IRequest
    {
        private static readonly DeleteServiceOptionCommandValidation s_validation = new();

        public Guid ServiceOptionId { get; }

        public DeleteServiceOptionCommand(Guid serviceOptionId) : base(Guid.NewGuid())
        {
            ServiceOptionId = serviceOptionId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
