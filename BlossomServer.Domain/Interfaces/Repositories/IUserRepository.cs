using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetUserByIdentifierAsync(string identifier);
    }
}
