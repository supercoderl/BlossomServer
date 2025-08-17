using BlossomServer.Domain.Errors;
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

namespace BlossomServer.Domain.Commands.Blogs.DeleteBlog
{
    public sealed class DeleteBlogCommandHandler : CommandHandlerBase, IRequestHandler<DeleteBlogCommand>
    {
        private readonly IBlogRepository _blogRepository;

        public DeleteBlogCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IBlogRepository blogRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _blogRepository = blogRepository;
        }

        public async Task Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
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

            _blogRepository.Remove(blog);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new BlogDeletedEvent(request.BlogId));
            }
        }
    }
}
