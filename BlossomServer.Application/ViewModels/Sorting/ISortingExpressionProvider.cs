using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Sorting
{
    public interface ISortingExpressionProvider<TViewModel, TEntity>
    {
        Dictionary<string, Expression<Func<TEntity, object>>> GetSortingExpressions();
    }
}
