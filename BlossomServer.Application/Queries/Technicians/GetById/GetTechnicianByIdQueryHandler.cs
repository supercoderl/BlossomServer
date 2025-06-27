using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.Technicians.GetById
{
    public sealed class GetTechnicianByIdQueryHandler :
        IRequestHandler<GetTechnicianByIdQuery, TechnicianViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly ITechnicianRepository _technicianRepository;

        public GetTechnicianByIdQueryHandler(ITechnicianRepository technicianRepository, IMediatorHandler bus)
        {
            _technicianRepository = technicianRepository;
            _bus = bus;
        }

        public async Task<TechnicianViewModel?> Handle(GetTechnicianByIdQuery request, CancellationToken cancellationToken)
        {
            var technician = await _technicianRepository.GetByIdAsync(request.Id);

            if (technician is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetTechnicianByIdQuery),
                        $"Technician with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return TechnicianViewModel.FromTechnician(technician);
        }
    }
}
