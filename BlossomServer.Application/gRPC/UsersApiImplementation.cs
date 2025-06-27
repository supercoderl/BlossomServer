using BlossomServer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.gRPC
{
    public sealed class UsersApiImplementation
    {
        private readonly IUserRepository _userRepository;

        public UsersApiImplementation(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

       /* public override async Task<GetUsersByIdsResult> GetByIds(
            GetUsersByIdsRequest request,
            ServerCallContext context)
        {
            var idsAsGuids = new List<Guid>(request.Ids.Count);

            foreach (var id in request.Ids)
            {
                if (Guid.TryParse(id, out var parsed))
                {
                    idsAsGuids.Add(parsed);
                }
            }

            var users = await _userRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Where(user => idsAsGuids.Contains(user.Id))
                .Select(user => new GrpcUser
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DeletedAt = user.DeletedAt == null ? "" : user.DeletedAt.ToString()
                })
                .ToListAsync();

            var result = new GetUsersByIdsResult();

            result.Users.AddRange(users);

            return result;
        }*/
    }
}
