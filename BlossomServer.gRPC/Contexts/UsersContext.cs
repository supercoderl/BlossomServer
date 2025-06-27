using BlossomServer.gRPC.Interfaces;
using BlossomServer.Shared.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.gRPC.Contexts
{
    public sealed class UsersContext : IUsersContext
    {
/*        private readonly UsersApi.UsersApiClient _client;

        public UsersContext(UsersApi.UsersApiClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersByIds(IEnumerable<Guid> ids)
        {
            var request = new GetUsersByIdsRequest();

            request.Ids.AddRange(ids.Select(id => id.ToString()));

            var result = await _client.GetByIdsAsync(request);

            return result.Users.Select(user => new UserViewModel(
                Guid.Parse(user.Id),
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                string.IsNullOrWhiteSpace(user.DeletedAt) ? null : DateTimeOffset.Parse(user.DeletedAt)));
        }*/
    }
}
