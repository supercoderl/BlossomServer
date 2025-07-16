using BlossomServer.Domain.Commands.BookingDetails.CreateBookingDetail;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Admin;
using BlossomServer.Shared.Events.Booking;
using BlossomServer.SharedKernel.Utils;
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

            if (!TimeZoneHelper.TryParseLocalDateTime(request.ScheduleTime, out var scheduleTime))
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"Schedule time is not correct format",
                    ErrorCodes.InsufficientPermissions
                ));
                return;
            }

            var booking = new Entities.Booking(
                request.BookingId,
                request.CustomerId,
                request.TechnicianId,
                scheduleTime,
                request.Price * request.Quantity,
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
                await Bus.RaiseEventAsync(new AdminNotificationRequiredEvent("You have a new booking!"));
                await Bus.SendCommandAsync(new CreateBookingDetailCommand(
                    Guid.NewGuid(),
                    booking.Id,
                    request.ServiceId,
                    request.ServiceOptionId,
                    request.Quantity,
                    request.Price
                ));
            }
        }
    }
}
