using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class BlogViewModelSortProvider : ISortingExpressionProvider<BlogViewModel, Blog>
    {
        private static readonly Dictionary<string, Expression<Func<Blog, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Blog, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
