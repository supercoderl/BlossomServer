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

namespace BlossomServer.Domain.Commands.Technicians.UpdateTechnician
{
    public sealed class UpdateTechnicianCommandHandler : CommandHandlerBase, IRequestHandler<UpdateTechnicianCommand>
    {
        private readonly ITechnicianRepository _technicianRepository;

        public UpdateTechnicianCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            ITechnicianRepository technicianRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _technicianRepository = technicianRepository;
        }

        public async Task Handle(UpdateTechnicianCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;

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

            technician.SetBio(request.Bio);
            technician.SetRating(request.Rating);
            technician.SetYearsOfExperience(request.YearsOfExperience);

            _technicianRepository.Update(technician);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new TechnicianUpdatedEvent(technician.Id));
            }
        }
    }
}
