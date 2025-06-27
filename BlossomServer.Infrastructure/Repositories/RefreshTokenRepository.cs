using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class RefreshTokenRepository : BaseRepository<RefreshToken, Guid>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<RefreshToken?> GetByToken(string token)
        {
            return await DbSet.Where(x => x.Token == token).SingleOrDefaultAsync();
        }
    }
}
