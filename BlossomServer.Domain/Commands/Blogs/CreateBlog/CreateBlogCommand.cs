using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.CreateBlog
{
    public sealed class CreateBlogCommand : CommandBase, IRequest
    {
        private static readonly CreateBlogCommandValidation s_validation = new();

        public Guid BlogId { get; }
        public string Title { get; }
        public string Slug { get; }
        public string Content { get; }
        public Guid AuthorId { get; }
        public string Tags { get; }
        public DateTime PublishedAt { get; }
        public bool IsPublished { get; }
        public string ThumbnailUrl { get; }

        public CreateBlogCommand(
            Guid blogId,
            string title,
            string slug,
            string content,
            Guid authorId,
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
            AuthorId = authorId;
            Tags = tags;
            PublishedAt = publishedAt;
            IsPublished = isPublished;
            ThumbnailUrl = thumbnailUrl;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
