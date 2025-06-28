using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Categories.GetAll;
using BlossomServer.Application.Queries.Categories.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Categories.CreateCategory;
using BlossomServer.Domain.Commands.Categories.DeleteCategory;
using BlossomServer.Domain.Commands.Categories.UpdateCategory;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMediatorHandler _bus;

        public CategoryService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> CreateCategoryAsync(CreateCategoryViewModel category)
        {
            var categoryId = Guid.NewGuid();

            await _bus.SendCommandAsync(new CreateCategoryCommand(
                categoryId,
                category.Name,
                category.Icon,
                category.Url,
                true,
                category.Priority
            ));

            return categoryId;
        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            await _bus.SendCommandAsync(new DeleteCategoryCommand(categoryId));
        }

        public async Task<PagedResult<CategoryViewModel>> GetAllCategoriesAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllCategoriesQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<CategoryViewModel?> GetCategoryByCategoryIdAsync(Guid categoryId)
        {
            return await _bus.QueryAsync(new GetCategoryByIdQuery(categoryId));
        }

        public async Task UpdateCategoryAsync(UpdateCategoryViewModel category)
        {
            await _bus.SendCommandAsync(new UpdateCategoryCommand(
                category.CategoryId,
                category.Name,
                category.IsActive,
                category.Icon,
                category.Url,
                category.Priority
            ));
        }
    }
}
