using BlossomServer.Application.ViewModels.Blogs;
using MediatR;

namespace BlossomServer.Application.Queries.Blogs.GetById
{
    public sealed record GetBlogByIdQuery(Guid Id) : IRequest<BlogViewModel?>;
}
