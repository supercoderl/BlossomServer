using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Blogs
{
    public sealed record CreateBlogViewModel
    (
        string Title,
        string Slug,
        string Content,
        string Tags,
        DateTime PublishedAt,
        bool IsPublished,
        IFormFile Thumbnail
    );
}
