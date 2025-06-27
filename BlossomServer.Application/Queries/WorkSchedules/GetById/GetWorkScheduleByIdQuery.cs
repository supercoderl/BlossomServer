using BlossomServer.Application.ViewModels.WorkSchedules;
using MediatR;

namespace BlossomServer.Application.Queries.WorkSchedules.GetById
{
    public sealed record GetWorkScheduleByIdQuery(Guid Id) : IRequest<WorkScheduleViewModel?>;
}
