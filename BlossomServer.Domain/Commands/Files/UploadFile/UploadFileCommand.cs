using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.UploadFile
{
    public sealed class UploadFileCommand : CommandBase, IRequest<string>
    {
        private static readonly UploadFileCommandValidation s_validation = new();

        public IFormFile File { get; }
        public string? ContentTypeOverride { get; }
        public bool ValidateChecksum { get; }

        public UploadFileCommand(
            IFormFile file,
            string? contentTypeOverride,
            bool validateChecksum
        ) : base(Guid.NewGuid())
        {
            File = file;
            ContentTypeOverride = contentTypeOverride;
            ValidateChecksum = validateChecksum;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
