using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryViewModel?> GetCategoryByCategoryIdAsync(Guid categoryId);

        public Task<PagedResult<CategoryViewModel>> GetAllCategoriesAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task<Guid> CreateCategoryAsync(CreateCategoryViewModel category);
        public Task UpdateCategoryAsync(UpdateCategoryViewModel category);
        public Task DeleteCategoryAsync(Guid categoryId);
    }
}
