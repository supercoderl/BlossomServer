using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Interfaces
{
    public interface ISignalRService
    {
        public Task SendData(string type, object data, string target, string? groupId = null, string? receiverId = null);
    }
}
