using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Blogs
{
    public sealed class BlogViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public string Tags { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsPublished { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
        public string AuthorName { get; set; } = string.Empty;

        public static BlogViewModel FromBlog(Blog blog)
        {
            return new BlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Slug = blog.Slug,
                Content = blog.Content,
                AuthorId = blog.AuthorId,
                Tags = blog.Tags,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                PublishedAt = blog.PublishedAt,
                IsPublished = blog.IsPublished,
                ThumbnailUrl = blog.ThumbnailUrl,
                ViewsCount = blog.ViewsCount,
                CommentsCount = blog.CommentsCount,
                AuthorName = blog.User?.FullName ?? string.Empty
            };
        }
    }
}
