using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class TechnicianViewModelSortProvider : ISortingExpressionProvider<TechnicianViewModel, Technician>
    {
        private static readonly Dictionary<string, Expression<Func<Technician, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Technician, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
