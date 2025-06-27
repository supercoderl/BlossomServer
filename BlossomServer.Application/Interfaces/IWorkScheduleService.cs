using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.WorkSchedules;

namespace BlossomServer.Application.Interfaces
{
    public interface IWorkScheduleService
    {
        public Task<WorkScheduleViewModel?> GetWorkScheduleByWorkScheduleIdAsync(Guid workScheduleId);

        public Task<PagedResult<WorkScheduleViewModel>> GetAllWorkSchedulesAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task<Guid> CreateWorkScheduleAsync(CreateWorkScheduleViewModel workSchedule);
        public Task UpdateWorkScheduleAsync(UpdateWorkScheduleViewModel workSchedule);
        public Task DeleteWorkScheduleAsync(Guid workScheduleId);
    }
}
