using BlossomServer.Application.ViewModels.Reviews;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class ReviewViewModelSortProvider : ISortingExpressionProvider<ReviewViewModel, Review>
    {
        private static readonly Dictionary<string, Expression<Func<Review, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Review, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
