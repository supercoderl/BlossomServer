using BlossomServer.Application.ViewModels.Categories;
using MediatR;

namespace BlossomServer.Application.Queries.Categories.GetById
{
    public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryViewModel?>;
}
