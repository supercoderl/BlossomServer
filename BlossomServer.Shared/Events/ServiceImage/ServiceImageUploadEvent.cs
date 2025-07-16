using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.ServiceImage
{
    public sealed class ServiceImageUploadProgressEvent : DomainEvent
    {
        public int ProgressPercentage { get; }
        public int CurrentFile { get; }
        public int TotalFiles { get; }
        public string CurrentFileName { get; }

        public ServiceImageUploadProgressEvent(
            Guid serviceId,
            int progressPercentage,
            int currentFile,
            int totalFiles,
            string currentFileName) : base(serviceId)
        {
            ProgressPercentage = progressPercentage;
            CurrentFile = currentFile;
            TotalFiles = totalFiles;
            CurrentFileName = currentFileName;
        }
    }
}
