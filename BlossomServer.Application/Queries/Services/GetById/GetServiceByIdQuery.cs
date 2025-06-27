using BlossomServer.Application.ViewModels.Services;
using MediatR;

namespace BlossomServer.Application.Queries.Services.GetById
{
    public sealed record GetServiceByIdQuery(Guid Id) : IRequest<ServiceViewModel?>;
}
