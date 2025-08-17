using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Payments.GetAll
{
    public sealed class GetAllPaymentsQueryHandler :
            IRequestHandler<GetAllPaymentsQuery, PagedResult<PaymentViewModel>>
    {
        private readonly ISortingExpressionProvider<PaymentViewModel, Payment> _sortingExpressionProvider;
        private readonly IPaymentRepository _paymentRepository;

        public GetAllPaymentsQueryHandler(
            IPaymentRepository paymentRepository,
            ISortingExpressionProvider<PaymentViewModel, Payment> sortingExpressionProvider)
        {
            _paymentRepository = paymentRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<PaymentViewModel>> Handle(
            GetAllPaymentsQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _paymentRepository.GetAllPaymentsBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var payments = results.Select(p => PaymentViewModel.FromPayment(p)).ToList();

            return new PagedResult<PaymentViewModel>(results.Count(), payments, request.Query.Page, request.Query.PageSize);
        }
    }
}
