using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.User;
using BlossomServer.SharedKernel.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ResetPassword
{
    public sealed class ResetPasswordCommandHandler : CommandHandlerBase, IRequestHandler<ResetPasswordCommand>
    {
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IPasswordResetTokenRepository passwordResetTokenRepository,
            IUserRepository userRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _userRepository = userRepository;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var passwordResetToken = await _passwordResetTokenRepository.GetByCode(request.Code);

            if(passwordResetToken == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Invalid or expired password reset token.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            var user = await _userRepository.GetByIdAsync(passwordResetToken.UserId);

            if (user == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "User not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            user.SetPassword(BCrypt.Net.BCrypt.HashPassword(request.NewPassword));

            _userRepository.Update(user);

            passwordResetToken.SetIsUsed(true);

            _passwordResetTokenRepository.Update(passwordResetToken);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ResetPasswordEvent(user.Id));
            }
        }
    }
}
