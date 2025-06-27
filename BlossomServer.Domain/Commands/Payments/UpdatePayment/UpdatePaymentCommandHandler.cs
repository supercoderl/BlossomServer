using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Payments.UpdatePayment
{
    public sealed class UpdatePaymentCommandHandler : CommandHandlerBase, IRequestHandler<UpdatePaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;

        public UpdatePaymentCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IPaymentRepository paymentRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);

            if (payment == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Payment not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            _paymentRepository.Update(payment);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new PaymentUpdatedEvent(request.PaymentId));
            }
        }
    }
}
