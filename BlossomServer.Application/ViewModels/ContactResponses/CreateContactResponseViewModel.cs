namespace BlossomServer.Application.ViewModels.ContactResponses
{
    public sealed record CreateContactResponseViewModel
    (
        Guid ContactId,
        string ResponseText
    );
}
