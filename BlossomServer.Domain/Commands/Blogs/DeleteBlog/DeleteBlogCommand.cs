using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.DeleteBlog
{
    public sealed class DeleteBlogCommand : CommandBase, IRequest
    {
        private static readonly DeleteBlogCommandValidation s_validation = new();

        public Guid BlogId { get; }

        public DeleteBlogCommand(Guid blogId) : base(Guid.NewGuid())
        {
            BlogId = blogId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
