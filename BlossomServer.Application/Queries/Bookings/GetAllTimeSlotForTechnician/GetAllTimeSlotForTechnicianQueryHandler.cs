using BlossomServer.Application.Queries.Bookings.GetAll;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Queries.Bookings.GetAllTimeSlotForTechnician
{
    public sealed class GetAllTimeSlotForTechnicianQueryHandler :
            IRequestHandler<GetAllTimeSlotForTechnicianQuery, IEnumerable<(DateTime start, TimeSpan duration)>>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetAllTimeSlotForTechnicianQueryHandler(
            IBookingRepository bookingRepository
        )
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<(DateTime start, TimeSpan duration)>> Handle(
            GetAllTimeSlotForTechnicianQuery request,
            CancellationToken cancellationToken)
        {
            return await _bookingRepository.GetScheduleTimes(request.technicianId, request.selectedDate);
        }
    }
}
