using BlossomServer.Domain.Commands.BookingDetails.CreateBookingDetail;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Admin;
using BlossomServer.Shared.Events.Booking;
using BlossomServer.SharedKernel.Utils;
using MassTransit;
using MediatR;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Bookings.CreateBooking
{
    public sealed class CreateBookingCommandHandler : CommandHandlerBase, IRequestHandler<CreateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateBookingCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IBookingRepository bookingRepository,
            ITechnicianRepository technicianRepository,
            IPublishEndpoint publishEndpoint
        ) : base(bus, unitOfWork, notifications)
        {
            _bookingRepository = bookingRepository;
            _technicianRepository = technicianRepository;
            _publishEndpoint = publishEndpoint;
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

            if (await CommitAsync())
            {
                await HandleBookingCreatedSuccessAsync(booking, request);
            }
        }

        private async Task HandleBookingCreatedSuccessAsync(Entities.Booking booking, CreateBookingCommand request)
        {
            var technician = await _technicianRepository.GetByIdAsync(booking.TechnicianId ?? Guid.Empty);

            // Create all events/commands in parallel where possible
            var tasks = new List<Task>
            {
                // Raise booking created event
                Bus.RaiseEventAsync(new BookingCreatedEvent(
                    technician?.UserId,
                    booking.Id,
                    booking.GuestEmail ?? "example@gmail.com",
                    booking.GuestName ?? string.Empty,
                    booking.GuestPhone ?? string.Empty,
                    booking.ScheduleTime,
                    request.ServiceId,
                    request.ServiceOptionId,
                    request.Quantity,
                    request.Price
                ))
            };

            await Task.WhenAll(tasks);
        }
    }
}
