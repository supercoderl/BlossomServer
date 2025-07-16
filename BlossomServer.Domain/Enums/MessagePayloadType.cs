using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Enums
{
    public enum MessagePayloadType
    {
        Chat,
        SystemMessage,
        Notification,
        Event,
        Progress
    }
}
