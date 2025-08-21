using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Bookings.GetScheduleByDate
{
    public sealed class GetScheduleByDateQueryHandler :
            IRequestHandler<GetScheduleByDateQuery, IEnumerable<object>>
    {
        private readonly IMediatorHandler _bus;
        private readonly IBookingRepository _bookingRepository;

        public GetScheduleByDateQueryHandler(IBookingRepository bookingRepository, IMediatorHandler bus)
        {
            _bookingRepository = bookingRepository;
            _bus = bus;
        }

        public async Task<IEnumerable<object>> Handle(GetScheduleByDateQuery request, CancellationToken cancellationToken)
        {
            var results = await _bookingRepository.GetScheduleByDateSQL(
                request.date,
                cancellationToken
            );

            return results;
        }
    }
}
