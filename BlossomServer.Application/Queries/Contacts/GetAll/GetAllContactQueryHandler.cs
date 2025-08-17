using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Contacts.GetAll
{
    public sealed class GetAllContactsQueryHandler :
            IRequestHandler<GetAllContactsQuery, PagedResult<ContactViewModel>>
    {
        private readonly ISortingExpressionProvider<ContactViewModel, Contact> _sortingExpressionProvider;
        private readonly IContactRepository _contactRepository;

        public GetAllContactsQueryHandler(
            IContactRepository contactRepository,
            ISortingExpressionProvider<ContactViewModel, Contact> sortingExpressionProvider)
        {
            _contactRepository = contactRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<ContactViewModel>> Handle(
            GetAllContactsQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _contactRepository.GetAllContactsBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var contacts = results.Select(c => ContactViewModel.FromContact(c)).ToList();

            return new PagedResult<ContactViewModel>(results.Count(), contacts, request.Query.Page, request.Query.PageSize);
        }
    }
}
