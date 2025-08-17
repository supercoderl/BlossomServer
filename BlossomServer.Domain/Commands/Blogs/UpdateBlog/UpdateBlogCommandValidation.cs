using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Blogs.UpdateBlog
{
    public sealed class UpdateBlogCommandValidation : AbstractValidator<UpdateBlogCommand>
    {
        public UpdateBlogCommandValidation()
        {
            
        }
    }
}
