using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Blogs.GetAll;
using BlossomServer.Application.Queries.Blogs.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Blogs.CreateBlog;
using BlossomServer.Domain.Commands.Blogs.DeleteBlog;
using BlossomServer.Domain.Commands.Blogs.UpdateBlog;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;
        private readonly IFileService _fileService;

        public BlogService(
            IMediatorHandler bus, 
            IUser user,
            IFileService fileService
        )
        {
            _bus = bus;
            _user = user;
            _fileService = fileService;
        }

        public async Task<Guid> CreateBlogAsync(CreateBlogViewModel blog)
        {
            var id = Guid.NewGuid();

            var imageUrl = await _fileService.UploadExampleFile(new ViewModels.Files.UploadExampleFileViewModel(blog.Thumbnail));

            await _bus.SendCommandAsync(new CreateBlogCommand(
                id,
                blog.Title,
                blog.Slug,
                blog.Content,
                _user.GetUserId(),
                blog.Tags,
                blog.PublishedAt,
                blog.IsPublished,
                imageUrl
            ));

            return id;
        }

        public async Task DeleteBlogAsync(Guid blogId)
        {
            await _bus.SendCommandAsync(new DeleteBlogCommand(blogId));
        }

        public async Task<PagedResult<BlogViewModel>> GetAllBlogsAsync(PageQuery query, bool includeDeleted, string searchTerm = "", bool isPublished = true, SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllBlogsQuery(query, includeDeleted, searchTerm, isPublished, sortQuery));
        }

        public async Task<BlogViewModel?> GetBlogByIdAsync(Guid blogId)
        {
            return await _bus.QueryAsync(new GetBlogByIdQuery(blogId));
        }

        public async Task UpdateBlogAsync(UpdateBlogViewModel blog)
        {
            string imageUrl = string.Empty;

            if (blog.Thumbnail != null)
            {
                imageUrl = await _fileService.UploadExampleFile(new ViewModels.Files.UploadExampleFileViewModel(blog.Thumbnail));
            }

            await _bus.SendCommandAsync(new UpdateBlogCommand(
                blog.BlogId,
                blog.Title,
                blog.Slug,
                blog.Content,
                blog.Tags,
                blog.PublishedAt,
                blog.IsPublished,
                imageUrl
            ));
        }
    }
}
