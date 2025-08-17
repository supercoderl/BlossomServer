using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.DeleteFile
{
    public sealed class DeleteFileCommand : CommandBase, IRequest
    {
        private static readonly DeleteFileCommandValidation s_validation = new();

        public string FileId { get; }

        public DeleteFileCommand(string fileId) : base(Guid.NewGuid())
        {
            FileId = fileId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
