using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class EmailReminderRepository : BaseRepository<EmailReminder, Guid>, IEmailReminderRepository
    {
        public EmailReminderRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<EmailReminder?> GetByEntityAndTypeAsync(Guid entityId, EmailReminderType reminderType, DateTime targetDate)
        {
            return await DbSet
            .FirstOrDefaultAsync(er =>
                er.EntityId == entityId &&
                er.ReminderType == reminderType &&
                er.TargetDate.Date == targetDate.Date);
        }
    }
}
