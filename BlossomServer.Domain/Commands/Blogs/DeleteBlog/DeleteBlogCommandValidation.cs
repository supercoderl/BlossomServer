using BlossomServer.Domain.Errors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.DeleteBlog
{
    public sealed class DeleteBlogCommandValidation : AbstractValidator<DeleteBlogCommand>
    {
        public DeleteBlogCommandValidation()
        {
            RuleForId();
        }

        public void RuleForId()
        {
            RuleFor(cmd => cmd.BlogId).NotEmpty().WithErrorCode(DomainErrorCodes.Blog.EmptyId).WithMessage("Id may not be empty.");
        }
    }
}
