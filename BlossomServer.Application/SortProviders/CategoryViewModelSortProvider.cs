using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class CategoryViewModelSortProvider : ISortingExpressionProvider<CategoryViewModel, Category>
    {
        private static readonly Dictionary<string, Expression<Func<Category, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Category, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
