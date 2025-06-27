using BlossomServer.Application.Extensions;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Application.Queries.Categories.GetAll
{
    public sealed class GetAllCategoriesQueryHandler :
        IRequestHandler<GetAllCategoriesQuery, PagedResult<CategoryViewModel>>
    {
        private readonly ISortingExpressionProvider<CategoryViewModel, Category> _sortingExpressionProvider;
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(
            ICategoryRepository categoryRepository,
            ISortingExpressionProvider<CategoryViewModel, Category> sortingExpressionProvider)
        {
            _categoryRepository = categoryRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<CategoryViewModel>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var categoriesQuery = _categoryRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await categoriesQuery.CountAsync(cancellationToken);

            categoriesQuery = categoriesQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            var categories = await categoriesQuery
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(category => CategoryViewModel.FromCategory(category))
                .ToListAsync(cancellationToken);

            return new PagedResult<CategoryViewModel>(
                totalCount, categories, request.Query.Page, request.Query.PageSize);
        }
    }
}
