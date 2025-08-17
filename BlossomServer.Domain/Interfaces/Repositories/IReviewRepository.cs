using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IReviewRepository : IRepository<Review, Guid>
    {
        Task<IEnumerable<Review>> GetAllReviewsBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );
    }
}
