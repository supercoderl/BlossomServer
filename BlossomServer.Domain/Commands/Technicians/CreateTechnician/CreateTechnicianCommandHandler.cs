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

namespace BlossomServer.Domain.Commands.Technicians.CreateTechnician
{
    public sealed class CreateTechnicianCommandHandler : CommandHandlerBase, IRequestHandler<CreateTechnicianCommand>
    {
        private readonly ITechnicianRepository _technicianRepository;

        public CreateTechnicianCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            ITechnicianRepository technicianRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _technicianRepository = technicianRepository;
        }

        public async Task Handle(CreateTechnicianCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;

            var technician = new Entities.Technician(
                request.TechnicianId,
                request.UserId,
                request.Bio,
                request.Rating,
                request.YearsOfExperience
            );

            _technicianRepository.Add(technician);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new TechnicianCreatedEvent(technician.Id));
            }
        }
    }
}
