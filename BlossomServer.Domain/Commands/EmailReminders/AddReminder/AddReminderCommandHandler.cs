using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.EmailReminders.AddReminder
{
    public sealed class AddReminderCommandHandler : CommandHandlerBase, IRequestHandler<AddReminderCommand>
    {
        private readonly IEmailReminderRepository _emailReminderRepository;

        public AddReminderCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IEmailReminderRepository emailReminderRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _emailReminderRepository = emailReminderRepository;
        }

        public async Task Handle(AddReminderCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var emailReminder = new Entities.EmailReminder(
                request.ReminderId,
                request.EntityId,
                request.RecipientEmail,
                request.RecipientName,
                request.RecipientType,
                request.ReminderType,
                request.Subject,
                request.Message,
                request.TargetDate,
                request.ReminderDate,
                request.IsScheduled,
                request.HangfireJobId
            );

            _emailReminderRepository.Add(emailReminder);

            await CommitAsync();
        }
    }
}
