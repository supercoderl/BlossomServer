using BlossomServer.Application.ViewModels.Notifications;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class NotificationViewModelSortProvider : ISortingExpressionProvider<NotificationViewModel, Notification>
    {
        private static readonly Dictionary<string, Expression<Func<Notification, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Notification, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
