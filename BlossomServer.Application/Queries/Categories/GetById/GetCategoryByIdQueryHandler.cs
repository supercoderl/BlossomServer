using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.Categories.GetById
{
    public sealed class GetCategoryByIdQueryHandler :
        IRequestHandler<GetCategoryByIdQuery, CategoryViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMediatorHandler bus)
        {
            _categoryRepository = categoryRepository;
            _bus = bus;
        }

        public async Task<CategoryViewModel?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetCategoryByIdQuery),
                        $"Category with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return CategoryViewModel.FromCategory(category);
        }
    }
}
