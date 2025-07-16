using BlossomServer.Application.ViewModels.Messages;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class MessageViewModelSortProvider : ISortingExpressionProvider<MessageViewModel, Message>
    {
        private static readonly Dictionary<string, Expression<Func<Message, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<Message, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
