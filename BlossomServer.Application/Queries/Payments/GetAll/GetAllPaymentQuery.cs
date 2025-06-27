using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;
using MediatR;

namespace BlossomServer.Application.Queries.Payments.GetAll
{
    public sealed record GetAllPaymentsQuery(
        PageQuery Query,
        bool IncludeDeleted,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<PaymentViewModel>>;
}
