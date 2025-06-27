using BlossomServer.Domain.Errors;
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

namespace BlossomServer.Domain.Commands.WorkSchedules.UpdateWorkSchedule
{
    public sealed class UpdateWorkScheduleCommandHandler : CommandHandlerBase, IRequestHandler<UpdateWorkScheduleCommand>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;

        public UpdateWorkScheduleCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IWorkScheduleRepository workScheduleRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _workScheduleRepository = workScheduleRepository;
        }

        public async Task Handle(UpdateWorkScheduleCommand request, CancellationToken cancellationToken)
        {
            if(!await TestValidityAsync(request)) return;

            var workSchedule = await _workScheduleRepository.GetByIdAsync(request.WorkScheduleId);

            if (workSchedule == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no work schedule with ID {request.WorkScheduleId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            workSchedule.SetWorkDate(request.WorkDate);
            workSchedule.SetStartTime(request.StartTime);
            workSchedule.SetEndTime(request.EndTime);
            workSchedule.SetIsDayOff(request.IsDayOff);

            _workScheduleRepository.Update(workSchedule);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new WorkScheduleUpdatedEvent(workSchedule.Id));
            }
        }
    }
}
