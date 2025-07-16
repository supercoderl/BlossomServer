using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Application.ViewModels.Users;

namespace BlossomServer.Application.Queries.Users.GetById
{
    public sealed class GetUserByIdQueryHandler :
        IRequestHandler<GetUserByIdQuery, UserViewModel?>
    {
        private readonly IMediatorHandler _bus;
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMediatorHandler bus)
        {
            _userRepository = userRepository;
            _bus = bus;
        }

        public async Task<UserViewModel?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                await _bus.RaiseEventAsync(
                    new DomainNotification(
                        nameof(GetUserByIdQuery),
                        $"User with id {request.Id} could not be found",
                        ErrorCodes.ObjectNotFound));
                return null;
            }

            return UserViewModel.FromUser(user, "web", null, null);
        }
    }
}
