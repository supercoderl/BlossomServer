using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System.Linq.Expressions;

namespace BlossomServer.Application.SortProviders
{
    public sealed class ContactViewModelSortProvider : ISortingExpressionProvider<ContactViewModel, Contact>
    {
        private static readonly Dictionary<string, Expression<Func<Contact, object>>> s_expressions = new()
        {
            { "name", contact => contact.Name },
            { "email", contact => contact.Email }
        };

        public Dictionary<string, Expression<Func<Contact, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
