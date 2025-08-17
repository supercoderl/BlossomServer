using BlossomServer.Application.Interfaces;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Blogs;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using BlossomServer.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/v1/[controller]")]
    public sealed class BlogController : ApiController
    {
        private readonly IBlogService _blogService;

        public BlogController(
            INotificationHandler<DomainNotification> notifications,
            IBlogService blogService) : base(notifications)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Get a list of all blogs")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<BlogViewModel>>))]
        public async Task<IActionResult> GetAllBlogsAsync(
            [FromQuery] PageQuery query,
            [FromQuery] string searchTerm = "",
            [FromQuery] bool includeDeleted = false,
            [FromQuery] bool isPublished = true,
            [FromQuery] [SortableFieldsAttribute<BlogViewModelSortProvider,BlogViewModel, Blog>]
        SortQuery? sortQuery = null)
        {
            var blogs = await _blogService.GetAllBlogsAsync(
                query,
                includeDeleted,
                searchTerm,
                isPublished,
                sortQuery);
            return Response(blogs);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation("Get a blog by id")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<BlogViewModel>))]
        public async Task<IActionResult> GetBlogByIdAsync([FromRoute] Guid id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            return Response(blog);
        }

        [HttpPost]
        [SwaggerOperation("Create a new blog")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> CreateBlogAsync([FromForm] CreateBlogViewModel viewModel)
        {
            var blogId = await _blogService.CreateBlogAsync(viewModel);
            return Response(blogId);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete a blog")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> DeleteBlogAsync([FromRoute] Guid id)
        {
            await _blogService.DeleteBlogAsync(id);
            return Response(id);
        }

        [HttpPut]
        [SwaggerOperation("Update a blog")]
        [SwaggerResponse(200, "Request successful")]
        public async Task<IActionResult> UpdateBlogAsync([FromForm] UpdateBlogViewModel viewModel)
        {
            await _blogService.UpdateBlogAsync(viewModel);
            return Response(viewModel);
        }
    }
}
