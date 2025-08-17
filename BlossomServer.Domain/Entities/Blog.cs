using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Blog : Entity<Guid>
    {
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string Content { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Tags { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime PublishedAt { get; private set; }
        public bool IsPublished { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public int ViewsCount { get; private set; }
        public int CommentsCount { get; private set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("Blogs")]
        public virtual User? User { get; set; }

        public Blog(
            Guid id,
            string title,
            string slug,
            string content,
            Guid authorId,
            string tags,
            DateTime publishedAt,
            bool isPublished,
            string thumbnailUrl
        ) : base(id)
        {
            Title = title;
            Slug = slug;
            Content = content;
            AuthorId = authorId;
            Tags = tags;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
            PublishedAt = publishedAt;
            IsPublished = isPublished;
            ThumbnailUrl = thumbnailUrl;
            ViewsCount = 0;
            CommentsCount = 0;
        }

        public void SetTitle(string title) { Title = title; }
        public void SetSlug(string slug) { Slug = slug; }
        public void SetContent(string content) { Content = content; }
        public void SetAuthorId(Guid authorId) { AuthorId = authorId; }
        public void SetTags(string tags) { Tags = tags; }
        public void SetPublishedAt(DateTime publishedAt) { PublishedAt = publishedAt; }
        public void SetUpdatedAt(DateTime? updatedAt) { UpdatedAt = updatedAt; }
        public void SetIsPublished(bool isPublished) { IsPublished = isPublished; }
        public void SetThumbnailUrl(string thumbnailUrl) { ThumbnailUrl = thumbnailUrl; }
        public void SetViewsCount(int viewsCount) { ViewsCount = viewsCount; }
        public void SetCommentsCount(int commentsCount) { CommentsCount = commentsCount; }
        public void SetUser(User? user) { User = user; }
        public void IncrementViewsCount() { ViewsCount++; }
        public void IncrementCommentsCount() { CommentsCount++; }
    }
}
