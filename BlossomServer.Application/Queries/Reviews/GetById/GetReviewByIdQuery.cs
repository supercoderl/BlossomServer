using BlossomServer.Application.ViewModels.Reviews;
using MediatR;

namespace BlossomServer.Application.Queries.Reviews.GetById
{
    public sealed record GetReviewByIdQuery(Guid Id) : IRequest<ReviewViewModel?>;
}
