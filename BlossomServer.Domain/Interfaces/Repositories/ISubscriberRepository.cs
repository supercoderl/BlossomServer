using BlossomServer.Domain.Entities;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface ISubscriberRepository : IRepository<Subscriber, Guid>
    {
        Task<Subscriber?> GetByEmail(string email, CancellationToken cancellationToken = default);
    }
}
