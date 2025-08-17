using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Mails.SendMail
{
    public sealed class SendMailCommand : CommandBase, IRequest
    {
        private static readonly SendMailCommandValidation s_validation = new();

        public string To { get; }
        public string Subject { get; }
        public string Content { get; }
        public bool IsHtml { get; }

        public SendMailCommand(
            string to,
            string subject,
            string content,
            bool isHtml
        ) : base(Guid.NewGuid())
        {
            To = to;
            Subject = subject;
            Content = content;
            IsHtml = isHtml;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
