using BlossomServer.Application.ViewModels;
using MediatR;

namespace BlossomServer.Application.Queries.Dashboards.Admin
{
    public sealed record GetBusinessAnalyticsQuery(
        PageQuery Query,
        DateRange CurrentRange,
        DateRange PreviousRange
    ) :
    IRequest<object>;
}
