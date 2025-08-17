using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.FileInfo
{
    public sealed class FileUploadedEvent : DomainEvent
    {
        public string FileId { get; set; }
        public string? OldUrl { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }

        public FileUploadedEvent(
            Guid fileUploadedId,
            string fileId,
            string? oldUrl,
            string url,
            string fileName
        ) : base(fileUploadedId)
        {
            FileId = fileId;
            OldUrl = oldUrl;
            Url = url;
            FileName = fileName;
        }
    }
}
