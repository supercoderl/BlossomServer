using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.WorkSchedules;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class WorkScheduleViewModelSortProvider : ISortingExpressionProvider<WorkScheduleViewModel, WorkSchedule>
    {
        private static readonly Dictionary<string, Expression<Func<WorkSchedule, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<WorkSchedule, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
