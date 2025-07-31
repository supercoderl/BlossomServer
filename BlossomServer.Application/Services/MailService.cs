using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Mails;
using BlossomServer.Domain.Commands.Mails.SendMail;
using BlossomServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Services
{
    public class MailService : IMailService
    {
        private readonly IMediatorHandler _bus;

        public MailService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task SendMailAsync(SendMailViewModel viewModel)
        {
            await _bus.SendCommandAsync(new SendMailCommand(viewModel.To, viewModel.Subject, viewModel.Content));
        }
    }
}
