using BlossomServer.Application.ViewModels.Promotions;
using MediatR;

namespace BlossomServer.Application.Queries.Promotions.GetById
{
    public sealed record GetPromotionByIdQuery(Guid Id) : IRequest<PromotionViewModel?>;
}
