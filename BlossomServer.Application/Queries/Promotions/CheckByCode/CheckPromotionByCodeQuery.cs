using MediatR;

namespace BlossomServer.Application.Queries.Promotions.CheckByCode
{
    public sealed record CheckPromotionByCodeQuery(string code) : IRequest<object>;
}
