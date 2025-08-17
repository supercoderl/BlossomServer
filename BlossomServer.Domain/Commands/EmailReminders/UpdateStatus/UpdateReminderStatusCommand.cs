using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.EmailReminders.UpdateStatus
{
    public sealed class UpdateReminderStatusCommand : CommandBase, IRequest
    {
        private static readonly UpdateReminderStatusCommandValidation s_validation = new();

        public Guid ReminderId { get; }
        public bool IsSent { get; }

        public UpdateReminderStatusCommand(
            Guid reminderId,
            bool isSent
        ) : base(Guid.NewGuid())
        {
            ReminderId = reminderId;
            IsSent = isSent;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
