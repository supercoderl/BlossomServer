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

namespace BlossomServer.Domain.Commands.WorkSchedules.DeleteWorkSchedule
{
    public sealed class DeleteWorkScheduleCommandHandler : CommandHandlerBase, IRequestHandler<DeleteWorkScheduleCommand>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;

        public DeleteWorkScheduleCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IWorkScheduleRepository workScheduleRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _workScheduleRepository = workScheduleRepository;
        }

        public async Task Handle(DeleteWorkScheduleCommand request, CancellationToken cancellationToken)
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

            _workScheduleRepository.Remove(workSchedule);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new WorkScheduleDeletedEvent(workSchedule.Id));
            }
        }
    }
}
