using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Settings
{
    public sealed class ImageKitSettings
    {
        public string PublicKey { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public int MaxFileSizeInBytes { get; set; } = 10485760; // 10 MB
    }
}
