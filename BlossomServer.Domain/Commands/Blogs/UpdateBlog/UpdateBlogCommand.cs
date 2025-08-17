using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.UpdateBlog
{
    public sealed class UpdateBlogCommand : CommandBase, IRequest
    {
        private static readonly UpdateBlogCommandValidation s_validation = new();

        public Guid BlogId { get; }
        public string Title { get; }
        public string Slug { get; }
        public string Content { get; }
        public string Tags { get; }
        public DateTime PublishedAt { get; }
        public bool IsPublished { get; }
        public string ThumbnailUrl { get; }

        public UpdateBlogCommand(
            Guid blogId,
            string title,
            string slug,
            string content,
            string tags,
            DateTime publishedAt,
            bool isPublished,
            string thumbnailUrl
        ) : base(Guid.NewGuid())
        {
            BlogId = blogId;
            Title = title;
            Slug = slug;
            Content = content;
            Tags = tags;
            PublishedAt = publishedAt;
            ThumbnailUrl = thumbnailUrl;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
