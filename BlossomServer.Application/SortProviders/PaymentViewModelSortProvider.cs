using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class PaymentViewModelSortProvider : ISortingExpressionProvider<PaymentViewModel, Payment>
    {
        private static readonly Dictionary<string, Expression<Func<Payment, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Payment, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
