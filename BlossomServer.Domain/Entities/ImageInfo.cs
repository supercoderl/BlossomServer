using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class FileInfo : Entity<Guid>
    {
        public string FileId { get; private set; }
        public string Url { get; private set; }
        public string FileName { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public FileInfo(
            Guid id,
            string fileId,
            string url,
            string fileName
        ) : base(id)
        {
            FileId = fileId;
            Url = url;
            FileName = fileName;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetFileId(string fileId) { FileId = fileId; }
        public void SetUrl(string url) { Url = url; }
        public void SetFileName(string fileName) { FileName = fileName; }
        public void SetCreatedAt(DateTime createdAt) { CreatedAt = createdAt; }
    }
}
