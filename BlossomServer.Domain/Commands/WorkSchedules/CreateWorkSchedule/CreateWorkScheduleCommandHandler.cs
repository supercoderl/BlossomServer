using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.WorkSchedule;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.WorkSchedules.CreateWorkSchedule
{
    public sealed class CreateWorkScheduleCommandHandler : CommandHandlerBase, IRequestHandler<CreateWorkScheduleCommand>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;

        public CreateWorkScheduleCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IWorkScheduleRepository workScheduleRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _workScheduleRepository = workScheduleRepository;
        }

        public async Task Handle(CreateWorkScheduleCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;

            var workSchedule = new Entities.WorkSchedule(
                request.WorkScheduleId,
                request.TechnicianId,
                request.WorkDate,
                request.StartTime,
                request.EndTime,
                request.IsDayOff
            );

            _workScheduleRepository.Add(workSchedule);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new WorkScheduleCreatedEvent(workSchedule.Id));
            }
        }
    }
}
