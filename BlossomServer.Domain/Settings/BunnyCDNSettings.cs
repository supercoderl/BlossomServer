using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Settings
{
    public sealed class BunnyCDNSettings
    {
        public string StorageZoneName { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string UploadPath { get; set; } = "/uploads";
        public int MaxFileSizeInBytes { get; set; } = 10485760; // 10 MB
        public string Region { get; set; } = string.Empty;
    }
}
