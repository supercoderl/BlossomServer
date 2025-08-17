using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Helpers;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using BlossomServer.Shared.Events.PasswordResetToken;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.ForgotPassword
{
    public sealed class ForgotPasswordCommandHandler : CommandHandlerBase, IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly ClientSettings _clientSettings;

        public ForgotPasswordCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IUserRepository userRepository,
            IPasswordResetTokenRepository passwordResetTokenRepository,
            IOptions<ClientSettings> settings
        ) : base(bus, unitOfWork, notifications)
        {
            _userRepository = userRepository;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _clientSettings = settings.Value;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var user = await _userRepository.GetUserByIdentifierAsync(request.Identifier);

            if(user == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "User not found with the provided identifier.",
                    ErrorCodes.ObjectNotFound
                ));

                return;
            }

            string code = TokenHelper.GenerateResetPasswordToken();

            var passwordResetToken = new PasswordResetToken(
                request.PasswordResetTokenId,
                user.Id,
                code
            );

            _passwordResetTokenRepository.Add(passwordResetToken);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new PasswordResetTokenCreatedEvent(passwordResetToken.Id));
                await Bus.SendCommandAsync(new SendMailCommand(
                    user.Email,
                    "Password Reset Request",
                    $""""
                        Dear {user.FullName},

                        Thank you for your request to reset your password.  
                        Please click the link below to proceed with resetting your password:

                        {_clientSettings.BaseUrl}/user/reset?code={code}

                        If you did not request this, please ignore this email.

                        Best regards,  
                        Blossom Nails Team
                    """",
                    false
                ));
            }
        }
    }
}
