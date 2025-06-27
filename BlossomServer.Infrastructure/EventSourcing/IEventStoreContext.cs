using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.EventSourcing
{
    public interface IEventStoreContext
    {
        public string GetUserEmail();
        public string GetCorrelationId();
    }
}
