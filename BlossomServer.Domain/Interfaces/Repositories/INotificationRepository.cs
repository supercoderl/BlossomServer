using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification, Guid>
    {
        public Task<IEnumerable<Notification>> GetAllNotificationsBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            Guid receiverId,
            UserRole role,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken
        );
    }
}
