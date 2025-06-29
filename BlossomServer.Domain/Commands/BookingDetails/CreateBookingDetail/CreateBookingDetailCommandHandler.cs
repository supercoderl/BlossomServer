using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.BookingDetail;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.BookingDetails.CreateBookingDetail
{
    public sealed class CreateBookingDetailCommandHandler : CommandHandlerBase, IRequestHandler<CreateBookingDetailCommand>
    {
        private readonly IBookingDetailRepository _bookingDetailRepository;

        public CreateBookingDetailCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IBookingDetailRepository bookingDetailRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _bookingDetailRepository = bookingDetailRepository;
        }

        public async Task Handle(CreateBookingDetailCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var bookingDetail = new Entities.BookingDetail(
                request.BookingDetailId,
                request.BookingId,
                request.ServiceId,
                request.ServiceOptionId,
                request.Quantity,
                request.UnitPrice
            );

            _bookingDetailRepository.Add( bookingDetail );

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new BookingDetailCreatedEvent(bookingDetail.Id));
            }
        }
    }
}
