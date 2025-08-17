using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using BlossomServer.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Mails.SendMail
{
    public sealed class SendMailCommandHandler : CommandHandlerBase, IRequestHandler<SendMailCommand>
    {
        private readonly MailSettings _mail;

        public SendMailCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notification,
            IOptions<MailSettings> settings
        ) : base( bus, unitOfWork, notification )
        {
            _mail = settings.Value;
        }

        public async Task Handle(SendMailCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = _mail.Port,
                Credentials = new NetworkCredential(_mail.Username, _mail.Password),
                EnableSsl = _mail.EnableSSL
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_mail.Username, _mail.DisplayName),
                Subject = request.Subject,
                Body = request.Content,
                IsBodyHtml = request.IsHtml
            };

            mail.To.Add(request.To);

            await client.SendMailAsync(mail, cancellationToken);
        }
    }
}
