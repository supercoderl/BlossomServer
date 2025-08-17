using BlossomServer.Domain.Commands.Files.UploadFile;
using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Services.UpdateService
{
    public sealed class UpdateServiceCommandHandler : CommandHandlerBase, IRequestHandler<UpdateServiceCommand>
    {
        private readonly IServiceRepository _serviceRepository;

        public UpdateServiceCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IServiceRepository serviceRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var service = await _serviceRepository.GetByIdAsync(request.ServiceId);

            if (service == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    "Service not found.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            service.SetName(request.Name);
            service.SetDescription(request.Description);
            service.SetPrice(request.Price);
            service.SetDurationMinutes(request.DurationMinutes);

            if(request.RepresentativeImage != null)
            {
                string url = await Bus.QueryAsync(new UploadFileCommand(
                    request.RepresentativeImage,
                    service.RepresentativeImage,
                    null,
                    false
                ));

                service.SetRepresentativeImage(url);
            }

            service.SetCategoryId(request.CategoryId);
            service.SetUpdatedAt(null);

            _serviceRepository.Update(service);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceUpdatedEvent(service.Id));
            }
        }
    }
}
