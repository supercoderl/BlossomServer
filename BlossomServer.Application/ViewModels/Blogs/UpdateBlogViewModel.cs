using Microsoft.AspNetCore.Http;

namespace BlossomServer.Application.ViewModels.Blogs
{
    public sealed record UpdateBlogViewModel
    (
        Guid BlogId,
        string Title,
        string Slug,
        string Content,
        string Tags,
        DateTime PublishedAt,
        bool IsPublished,
        IFormFile? Thumbnail
    );
}
