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

namespace BlossomServer.Domain.Commands.EmailReminders.UpdateStatus
{
    public sealed class UpdateReminderStatusCommandHandler : CommandHandlerBase, IRequestHandler<UpdateReminderStatusCommand>
    {
        private readonly IEmailReminderRepository _emailReminderRepository;

        public UpdateReminderStatusCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IEmailReminderRepository emailReminderRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _emailReminderRepository = emailReminderRepository;
        }

        public async Task Handle(UpdateReminderStatusCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var emailReminder = await _emailReminderRepository.GetByIdAsync(request.ReminderId);

            if (emailReminder == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no any reminder with id {request.ReminderId}.",
                    ErrorCodes.ObjectNotFound
                ));

                return;
            }

            emailReminder.SetIsSent(request.IsSent);

            _emailReminderRepository.Update(emailReminder);

            await CommitAsync();
        }
    }
}
