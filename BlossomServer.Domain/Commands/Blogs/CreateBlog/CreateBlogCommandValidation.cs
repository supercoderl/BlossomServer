using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.CreateBlog
{
    public sealed class CreateBlogCommandValidation : AbstractValidator<CreateBlogCommand>
    {
        public CreateBlogCommandValidation()
        {
            
        }
    }
}
