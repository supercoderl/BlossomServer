using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

namespace BlossomServer.Application.Queries.Blogs.GetAll
{
    public sealed class GetAllBlogsQueryHandler :
            IRequestHandler<GetAllBlogsQuery, PagedResult<BlogViewModel>>
    {
        private readonly ISortingExpressionProvider<BlogViewModel, Blog> _sortingExpressionProvider;
        private readonly IBlogRepository _blogRepository;

        public GetAllBlogsQueryHandler(
            IBlogRepository blogRepository,
            ISortingExpressionProvider<BlogViewModel, Blog> sortingExpressionProvider)
        {
            _blogRepository = blogRepository;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<BlogViewModel>> Handle(
            GetAllBlogsQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _blogRepository.GetAllBlogsBySQL(
                request.SearchTerm,
                request.IncludeDeleted,
                request.Query.Page,
                request.Query.PageSize,
                request.IsPublished,
                request.SortQuery?.Query ?? "Id",
                "ASC",
                cancellationToken
            );

            var blogs = results.Select(b => BlogViewModel.FromBlog(b)).ToList();

            return new PagedResult<BlogViewModel>(results.Count(), blogs, request.Query.Page, request.Query.PageSize);
        }
    }
}
