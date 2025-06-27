using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.UploadFile
{
    public sealed class UploadFileCommandValidation : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidation()
        {
            
        }

        public void RuleForFile()
        {
            RuleFor(cmd => cmd.File)
                .NotNull()
                .WithMessage("File must not be null.")
                .Must(file => file.Length > 0)
                .WithMessage("File must not be empty.");
        }
    }
}
