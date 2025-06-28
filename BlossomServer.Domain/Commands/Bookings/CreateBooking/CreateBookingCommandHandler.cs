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

namespace BlossomServer.Domain.Commands.Bookings.CreateBooking
{
    public sealed class CreateBookingCommandHandler : CommandHandlerBase, IRequestHandler<CreateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;

        public CreateBookingCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IBookingRepository bookingRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var booking = new Entities.Booking(
                request.BookingId,
                request.CustomerId,
                request.TechnicianId,
                request.ScheduleTime,
                request.TotalPrice,
                Enums.BookingStatus.Pending,
                request.Note,
                request.GuestName,
                request.GuestPhone,
                request.GuestEmail
            );

            _bookingRepository.Add(booking);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new BookingCreatedEvent(booking.Id));
            }
        }
    }
}
