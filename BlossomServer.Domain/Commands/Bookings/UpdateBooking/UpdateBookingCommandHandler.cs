using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Booking;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.UpdateBooking
{
    public sealed class UpdateBookingCommandHandler : CommandHandlerBase, IRequestHandler<UpdateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;

        public UpdateBookingCommandHandler(
           IMediatorHandler bus,
           IUnitOfWork unitOfWork,
           INotificationHandler<DomainNotification> notifications,
           IBookingRepository bookingRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            if (booking == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no booking with ID {request.BookingId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            booking.SetCustomerId(request.CustomerId);
            booking.SetTechnicianId(request.TechnicianId);
            booking.SetScheduleTime(request.ScheduleTime);
            booking.SetTotalPrice(request.TotalPrice);
            booking.SetStatus(request.Status);
            booking.SetNote(request.Note);
            booking.SetGuestName(request.GuestName);
            booking.SetGuestPhone(request.GuestPhone);
            booking.SetGuestEmail(request.GuestEmail);
            booking.SetUpdatedAt();

            _bookingRepository.Update(booking);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new BookingUpdatedEvent(request.BookingId));
            }
        }
    }
}
