using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Blog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.CreateBlog
{
    public sealed class CreateBlogCommandHandler : CommandHandlerBase, IRequestHandler<CreateBlogCommand>
    {
        private readonly IBlogRepository _blogRepository;

        public CreateBlogCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IBlogRepository blogRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _blogRepository = blogRepository;
        }

        public async Task Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var blog = new Entities.Blog(
                request.BlogId,
                request.Title,
                request.Slug,
                request.Content,
                request.AuthorId,
                request.Tags,
                request.PublishedAt,
                request.IsPublished,
                request.ThumbnailUrl
            );

            _blogRepository.Add(blog);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new BlogCreatedEvent(blog.Id));
            }
        }
    }
}
