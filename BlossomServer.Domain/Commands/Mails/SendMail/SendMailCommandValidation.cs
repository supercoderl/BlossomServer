using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Mails.SendMail
{
    public sealed class SendMailCommandValidation : AbstractValidator<SendMailCommand>
    {
        public SendMailCommandValidation()
        {
            RuleForTo();
            RuleForSubject();
            RuleForContent();
        }

        public void RuleForTo()
        {
            RuleFor(cmd => cmd.To).NotEmpty().WithMessage("Receiver may not be empty.");
        }

        public void RuleForSubject()
        {
            RuleFor(cmd => cmd.Subject).NotEmpty().WithMessage("Subject may not be empty.");
        }

        public void RuleForContent()
        {
            RuleFor(cmd => cmd.Content).NotEmpty().WithMessage("Content may not be empty.");
        }
    }
}
