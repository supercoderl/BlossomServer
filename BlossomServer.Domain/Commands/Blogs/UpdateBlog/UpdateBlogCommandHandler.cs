using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Blog;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.UpdateBlog
{
    public sealed class UpdateBlogCommandHandler : CommandHandlerBase, IRequestHandler<UpdateBlogCommand>
    {
        private readonly IBlogRepository _blogRepository;

        public UpdateBlogCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IBlogRepository blogRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _blogRepository = blogRepository;
        }

        public async Task Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var blog = await _blogRepository.GetByIdAsync(request.BlogId);

            if(blog == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no any blog with id {request.BlogId}.",
                    ErrorCodes.ObjectNotFound
                ));

                return;
            }

            blog.SetTitle(request.Title);
            blog.SetSlug(request.Slug);
            blog.SetContent(request.Content);
            blog.SetTags(request.Tags);
            blog.SetPublishedAt(request.PublishedAt);
            blog.SetIsPublished(request.IsPublished);
            if(!string.IsNullOrEmpty(request.ThumbnailUrl))
            {
                blog.SetThumbnailUrl(request.ThumbnailUrl);
            }
            blog.SetUpdatedAt(TimeZoneHelper.GetLocalTimeNow());

            _blogRepository.Update(blog);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new BlogUpdatedEvent(request.BlogId));
            }
        }
    }
}
