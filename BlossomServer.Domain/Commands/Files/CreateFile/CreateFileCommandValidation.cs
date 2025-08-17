using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.CreateFile
{
    public sealed class CreateFileCommandValidation : AbstractValidator<CreateFileCommand>
    {
        public CreateFileCommandValidation()
        {
            
        }
    }
}
