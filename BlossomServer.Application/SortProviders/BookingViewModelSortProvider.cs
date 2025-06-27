using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class BookingViewModelSortProvider : ISortingExpressionProvider<BookingViewModel, Booking>
    {
        private static readonly Dictionary<string, Expression<Func<Booking, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Booking, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
