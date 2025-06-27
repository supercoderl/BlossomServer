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

namespace BlossomServer.Domain.Commands.Payments.CreatePayment
{
    public sealed class CreatePaymentCommandHandler : CommandHandlerBase, IRequestHandler<CreatePaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;

        public CreatePaymentCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IPaymentRepository paymentRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var payment = new Entities.Payment(
                request.PaymentId,
                request.BookingId,
                request.Amount,
                request.Method,
                request.TransactionCode
            );

            _paymentRepository.Add(payment);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new PaymentCreatedEvent(payment.Id));
            }
        }
    }
}
