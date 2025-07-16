using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class PromotionRepository : BaseRepository<Promotion, Guid>, IPromotionRepository
    {
        public PromotionRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Promotion?> CheckByCode(string code)
        {
            return await DbSet.SingleOrDefaultAsync(c => c.Code == code);
        }
    }
}
