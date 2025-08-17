using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Contacts.GetAll
{
    public sealed record GetAllContactsQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<ContactViewModel>>;
}
