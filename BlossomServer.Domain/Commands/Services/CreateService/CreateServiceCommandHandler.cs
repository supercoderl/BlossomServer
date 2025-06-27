using BlossomServer.Domain.Commands.Files.UploadFile;
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

namespace BlossomServer.Domain.Commands.Services.CreateService
{
    public sealed class CreateServiceCommandHandler : CommandHandlerBase, IRequestHandler<CreateServiceCommand>
    {
        private readonly IServiceRepository _serviceRepository;

        public CreateServiceCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IServiceRepository serviceRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var url = await Bus.QueryAsync(new UploadFileCommand(
                request.RepresentativeImage,
                null,
                false
            ));

            var service = new Entities.Service(
                request.ServiceId,
                request.Name,
                request.Description,
                request.CategoryId,
                request.Price,
                request.DurationMinutes,
                url
            );

            _serviceRepository.Add(service);

            if (await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceCreatedEvent(service.Id));
            }
        }
    }
}
