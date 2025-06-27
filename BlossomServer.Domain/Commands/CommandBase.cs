using BlossomServer.SharedKernel.Utils;
using FluentValidation.Results;
using MediatR;

namespace BlossomServer.Domain.Commands
{
    public abstract class CommandBase :IRequest
    {
        public Guid AggregateId { get; }
        public string MessageType { get; }
        public DateTime Timestamp { get; }
        public ValidationResult? ValidationResult { get; protected set; }

        protected CommandBase(Guid aggregateId)
        {
            AggregateId = aggregateId;
            MessageType = GetType().Name;
            Timestamp = TimeZoneHelper.GetLocalTimeNow();
        }

        public abstract bool IsValid();
    }
}
