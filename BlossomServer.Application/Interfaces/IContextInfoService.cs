using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Interfaces
{
    public interface IContextInfoService
    {
        public Task SetContextAsync(string userId, string? ipAddress = null, string? userAgent = null, string? sessionId = null, CancellationToken cancellationToken = default);
        public Task ClearContextAsync(CancellationToken cancellationToken = default);
    }
}
