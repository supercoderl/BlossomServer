using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IServiceRepository : IRepository<Service, Guid>
    {
        Task<IEnumerable<Service>> GetAllServicesBySQL(
            string searchTerm,
            bool includeDeleted,
            int page,
            int pageSize,
            string sortColumn,
            string sortDirection,
            CancellationToken cancellationToken = default
        );
        Task<IEnumerable<object>> GetServicesPopularityRanking(string currentDateStart, string currentDateEnd, string previousDateStart, string previousDateEnd, CancellationToken cancellationToken);
        Task<decimal> GetAverageServiceValue(string dateStart, string dateEnd, CancellationToken cancellationToken = default);
    }
}
