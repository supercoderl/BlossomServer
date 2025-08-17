using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.CreateFile
{
    public sealed class CreateFileCommand : CommandBase, IRequest
    {
        private static readonly CreateFileCommandValidation s_validation = new();

        public Guid FileInfoId { get; }
        public string FileId { get; }
        public string Url { get; }
        public string FileName { get; }

        public CreateFileCommand(
            Guid fileInfoId,
            string fileId,
            string url,
            string fileName
        ) : base(Guid.NewGuid())
        {
            FileInfoId = fileInfoId;
            FileId = fileId;
            Url = url;
            FileName = fileName;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
