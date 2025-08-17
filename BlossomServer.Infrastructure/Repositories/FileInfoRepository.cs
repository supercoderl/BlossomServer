using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class FileInfoRepository : BaseRepository<Domain.Entities.FileInfo, Guid>, IFileInfoRepository
    {
        public FileInfoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Domain.Entities.FileInfo?> GetByUrl(string url)
        {
            return await DbSet.SingleOrDefaultAsync(x => x.Url.Equals(url));
        }
    }
}
