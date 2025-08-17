using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog, Guid>
    {
    }
}
