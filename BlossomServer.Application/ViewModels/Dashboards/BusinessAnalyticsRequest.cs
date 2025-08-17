namespace BlossomServer.Application.ViewModels.Dashboards
{
    public sealed record BusinessAnalyticsRequest
    (
        PageQuery Query,
        DateRange CurrentRange,
        DateRange PreviousRange
    );
}
