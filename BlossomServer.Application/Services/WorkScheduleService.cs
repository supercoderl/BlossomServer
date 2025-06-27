using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.WorkSchedules.GetAll;
using BlossomServer.Application.Queries.WorkSchedules.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.WorkSchedules;
using BlossomServer.Domain.Commands.WorkSchedules.CreateWorkSchedule;
using BlossomServer.Domain.Commands.WorkSchedules.DeleteWorkSchedule;
using BlossomServer.Domain.Commands.WorkSchedules.UpdateWorkSchedule;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly IMediatorHandler _bus;

        public WorkScheduleService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> CreateWorkScheduleAsync(CreateWorkScheduleViewModel workSchedule)
        {
            var workScheduleId = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateWorkScheduleCommand(
                workScheduleId,
                workSchedule.TechnicianId,
                workSchedule.WorkDate,
                workSchedule.StartTime,
                workSchedule.EndTime,
                workSchedule.IsDayOff
            ));

            return workScheduleId;
        }

        public async Task DeleteWorkScheduleAsync(Guid workScheduleId)
        {
            await _bus.SendCommandAsync(new DeleteWorkScheduleCommand(workScheduleId));
        }

        public async Task<PagedResult<WorkScheduleViewModel>> GetAllWorkSchedulesAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllWorkSchedulesQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<WorkScheduleViewModel?> GetWorkScheduleByWorkScheduleIdAsync(Guid workScheduleId)
        {
            return await _bus.QueryAsync(new GetWorkScheduleByIdQuery(workScheduleId));
        }

        public async Task UpdateWorkScheduleAsync(UpdateWorkScheduleViewModel workSchedule)
        {
            await _bus.SendCommandAsync(new UpdateWorkScheduleCommand(
                workSchedule.WorkScheduleId,
                workSchedule.WorkDate,
                workSchedule.StartTime,
                workSchedule.EndTime,
                workSchedule.IsDayOff
            ));
        }
    }
}
