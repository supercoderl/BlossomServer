using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Categories;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

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
            var results = await _categoryRepository.GetAllCategoriesBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var categories = results.Select(c => CategoryViewModel.FromCategory(c)).ToList();

            return new PagedResult<CategoryViewModel>(results.Count(), categories, request.Query.Page, request.Query.PageSize);
        }
    }
}
