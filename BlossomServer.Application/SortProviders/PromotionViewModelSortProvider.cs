using BlossomServer.Application.ViewModels.Promotions;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class PromotionViewModelSortProvider : ISortingExpressionProvider<PromotionViewModel, Promotion>
    {
        private static readonly Dictionary<string, Expression<Func<Promotion, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Promotion, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
