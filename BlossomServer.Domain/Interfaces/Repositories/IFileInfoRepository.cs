namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IFileInfoRepository : IRepository<Entities.FileInfo, Guid>
    {
        Task<Entities.FileInfo?> GetByUrl(string url);
    }
}
