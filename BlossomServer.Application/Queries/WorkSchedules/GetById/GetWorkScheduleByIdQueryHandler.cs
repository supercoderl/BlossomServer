using BlossomServer.Application.ViewModels.WorkSchedules;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.WorkSchedules.GetById
{
    public sealed class GetWorkScheduleByIdQueryHandler :
        IRequestHandler<GetWorkScheduleByIdQuery, WorkScheduleViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly IWorkScheduleRepository _workScheduleRepository;

        public GetWorkScheduleByIdQueryHandler(IWorkScheduleRepository workScheduleRepository, IMediatorHandler bus)
        {
            _workScheduleRepository = workScheduleRepository;
            _bus = bus;
        }

        public async Task<WorkScheduleViewModel?> Handle(GetWorkScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var workSchedule = await _workScheduleRepository.GetByIdAsync(request.Id);

            if (workSchedule is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetWorkScheduleByIdQuery),
                        $"WorkSchedule with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return WorkScheduleViewModel.FromWorkSchedule(workSchedule);
        }
    }
}
