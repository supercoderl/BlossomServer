using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Booking;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.UpdateBookingStatus
{
    public sealed class UpdateBookingStatusCommandHandler : CommandHandlerBase, IRequestHandler<UpdateBookingStatusCommand>
    {
        private readonly IBookingRepository _bookingRepository;

        public UpdateBookingStatusCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IBookingRepository bookingRepository
        ) : base( bus, unitOfWork, notifications )
        {
            _bookingRepository = bookingRepository;
        }

        public async Task Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            if(booking == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no any booking with id {request.BookingId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            booking.SetStatus(request.Status);

            _bookingRepository.Update(booking);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new BookingUpdatedEvent(request.BookingId));
            }
        }
    }
}
