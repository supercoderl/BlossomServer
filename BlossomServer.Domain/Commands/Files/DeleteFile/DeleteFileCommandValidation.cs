using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.DeleteFile
{
    public sealed class DeleteFileCommandValidation : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidation()
        {
            RuleForId();
        }

        public void RuleForId()
        {
            RuleFor(cmd => cmd.FileId).NotEmpty().WithMessage("File id may not be empty.");
        }
    }
}
