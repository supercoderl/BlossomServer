using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.BackgroundServices
{
    public interface IEmailReminderBackgroundService
    {
        Task ProcessEmailRemindersAsync();
        Task SendScheduledEmailAsync(Guid emailReminderId);
    }
}
