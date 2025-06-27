using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class ServiceViewModelSortProvider : ISortingExpressionProvider<ServiceViewModel, Service>
    {
        private static readonly Dictionary<string, Expression<Func<Service, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Service, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
