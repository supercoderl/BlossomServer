using BlossomServer.Application.ViewModels.Mails;

namespace BlossomServer.Application.Interfaces
{
    public interface IMailService
    {
        public Task SendMailAsync(SendMailViewModel viewModel);
    }
}
