using BlossomServer.Application.Queries.Bookings.GetById;
using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;

namespace BlossomServer.Application.Queries.Blogs.GetById
{
    public sealed class GetBlogByIdQueryHandler :
            IRequestHandler<GetBlogByIdQuery, BlogViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly IBlogRepository _blogRepository;

        public GetBlogByIdQueryHandler(IBlogRepository blogRepository, IMediatorHandler bus)
        {
            _blogRepository = blogRepository;
            _bus = bus;
        }

        public async Task<BlogViewModel?> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
            var blog = await _blogRepository.GetByIdAsync(request.Id);

            if (blog is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetBookingByIdQuery),
                        $"Blog with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return BlogViewModel.FromBlog(blog);
        }
    }
}
