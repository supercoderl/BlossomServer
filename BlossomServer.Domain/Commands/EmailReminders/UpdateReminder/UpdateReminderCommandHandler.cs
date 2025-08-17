using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.EmailReminders.UpdateReminder
{
    public sealed class UpdateReminderCommandHandler : CommandHandlerBase, IRequestHandler<UpdateReminderCommand>
    {
        private readonly IEmailReminderRepository _emailReminderRepository;

        public UpdateReminderCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IEmailReminderRepository emailReminderRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _emailReminderRepository = emailReminderRepository;
        }

        public async Task Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var emailReminder = await _emailReminderRepository.GetByIdAsync(request.ReminderId);

            if(emailReminder == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.Message,
                    $"There is no any reminder with id {request.ReminderId}.",
                    ErrorCodes.ObjectNotFound
                ));

                return;
            }

            emailReminder.SetEntityId(request.ReminderId);
            emailReminder.SetRecipientEmail(request.RecipientEmail);
            emailReminder.SetRecipientName(request.RecipientName);
            emailReminder.SetRecipientType(request.RecipientType);
            emailReminder.SetReminderType(request.ReminderType);
            emailReminder.SetSubject(request.Subject);
            emailReminder.SetMessage(request.Message);
            emailReminder.SetTargetDate(request.TargetDate);
            emailReminder.SetReminderDate(request.ReminderDate);
            emailReminder.SetIsScheduled(request.IsScheduled);
            emailReminder.SetIsSent(emailReminder.IsSent);
            emailReminder.SetHangfireJobId(request.HangfireJobId);

            _emailReminderRepository.Update(emailReminder);

            await CommitAsync();
        }
    }
}
