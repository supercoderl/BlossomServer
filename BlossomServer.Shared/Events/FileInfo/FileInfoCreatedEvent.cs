using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.FileInfo
{
    public sealed class FileInfoCreatedEvent : DomainEvent
    {
        public FileInfoCreatedEvent(Guid fileInfoId) : base(fileInfoId)
        {
            
        }
    }
}
