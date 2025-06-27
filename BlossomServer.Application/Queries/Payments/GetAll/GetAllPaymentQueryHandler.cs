using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var paymentsQuery = _paymentRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await paymentsQuery.CountAsync(cancellationToken);

            paymentsQuery = paymentsQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var payments = await paymentsQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(payment => PaymentViewModel.FromPayment(payment))
                .ToListAsync(cancellationToken);

            return new PagedResult<PaymentViewModel>(
                totalCount, payments, request.Query.Page, request.Query.PageSize);
        }
    }
}
