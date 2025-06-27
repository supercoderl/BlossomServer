using BlossomServer.Domain.Commands.Files.UploadFile;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.ServiceImage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceImages.CreateServiceImage
{
    public sealed class CreateServiceImageCommandHandler : CommandHandlerBase, IRequestHandler<CreateServiceImageCommand>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public CreateServiceImageCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IServiceImageRepository serviceImageRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _serviceImageRepository = serviceImageRepository;
        }

        public async Task Handle(CreateServiceImageCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var url = await Bus.QueryAsync(new UploadFileCommand(
                request.ImageFile,
                null,
                false
            ));

            var serviceImage = new Entities.ServiceImage(
                request.ServiceImageId,
                request.ImageFile.FileName,
                url,
                request.ServiceId,
                request.Description
            );

            _serviceImageRepository.Add(serviceImage);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceImageCreatedEvent(serviceImage.Id));
            }
        }
    }
}
