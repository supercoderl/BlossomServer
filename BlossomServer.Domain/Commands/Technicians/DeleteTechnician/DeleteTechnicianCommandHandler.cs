using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Technician;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Technicians.DeleteTechnician
{
    public sealed class DeleteTechnicianCommandHandler : CommandHandlerBase, IRequestHandler<DeleteTechnicianCommand>
    {
        private readonly ITechnicianRepository _technicianRepository;

        public DeleteTechnicianCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            ITechnicianRepository technicianRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _technicianRepository = technicianRepository;
        }

        public async Task Handle(DeleteTechnicianCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var technician = await _technicianRepository.GetByIdAsync(request.TechnicianId);

            if (technician == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Technician not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            _technicianRepository.Remove(technician);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new TechnicianDeletedEvent(technician.Id));
            }
        }
    }
}
