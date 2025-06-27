using BlossomServer.Application.ViewModels.Technicians;
using MediatR;

namespace BlossomServer.Application.Queries.Technicians.GetById
{
    public sealed record GetTechnicianByIdQuery(Guid Id) : IRequest<TechnicianViewModel?>;
}
