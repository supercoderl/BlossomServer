using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IBlogService
    {
        public Task<BlogViewModel?> GetBlogByIdAsync(Guid blogId);

        public Task<PagedResult<BlogViewModel>> GetAllBlogsAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            bool isPublished = true,
            SortQuery? sortQuery = null);

        public Task<Guid> CreateBlogAsync(CreateBlogViewModel blog);
        public Task UpdateBlogAsync(UpdateBlogViewModel blog);
        public Task DeleteBlogAsync(Guid blogId);
    }
}
