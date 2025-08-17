using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IEmailReminderRepository : IRepository<EmailReminder, Guid>
    {
        Task<EmailReminder?> GetByEntityAndTypeAsync(Guid entityId, EmailReminderType reminderType, DateTime targetDate);
    }
}
