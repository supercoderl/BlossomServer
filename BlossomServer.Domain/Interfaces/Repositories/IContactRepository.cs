using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IContactRepository : IRepository<Contact, Guid>
    {
        Task<IEnumerable<Contact>> GetAllContactsBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<Contact>> GetAllContactsByEmailSQL(
            bool includeResponses,
            string email,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );
    }
}
