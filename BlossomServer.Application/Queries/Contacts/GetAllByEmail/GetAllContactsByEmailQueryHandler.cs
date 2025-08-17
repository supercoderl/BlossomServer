using BlossomServer.Application.Queries.Contacts.GetAll;
using BlossomServer.Application.ViewModels.Contacts;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Domain.Entities;

namespace BlossomServer.Application.Queries.Contacts.GetAllByEmail
{
    public sealed class GetAllContactsByEmailQueryHandler :
                IRequestHandler<GetAllContactsByEmailQuery, PagedResult<ContactViewModel>>
    {
        private readonly ISortingExpressionProvider<ContactViewModel, Contact> _sortingExpressionProvider;
        private readonly IContactRepository _contactRepository;

        public GetAllContactsByEmailQueryHandler(
            IContactRepository contactRepository,
            ISortingExpressionProvider<ContactViewModel, Contact> sortingExpressionProvider)
        {
            _contactRepository = contactRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<ContactViewModel>> Handle(
            GetAllContactsByEmailQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _contactRepository.GetAllContactsByEmailSQL(
                request.IncludeResponses,
                request.Email,
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
