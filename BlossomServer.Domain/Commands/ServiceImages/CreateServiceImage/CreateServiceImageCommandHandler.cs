using BlossomServer.Domain.Commands.Files.UploadFile;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.ServiceImage;
using MediatR;

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

            List<Entities.ServiceImage> serviceImages = new List<Entities.ServiceImage>();

            int total = request.ImageFile.Count;

            for (int i = 0; i < total; i++)
            {
                var file = request.ImageFile[i];

                var url = await Bus.QueryAsync(new UploadFileCommand(file, null, false));
                var serviceImage = new Entities.ServiceImage(
                    request.ServiceImageId,
                    file.FileName,
                    url,
                    request.ServiceId,
                    request.Description
                );

                serviceImages.Add(serviceImage);

                int percent = (int)(((i + 1) / (double)total) * 100);

                await Bus.RaiseEventAsync(new ServiceImageUploadProgressEvent(
                    request.ServiceId,
                    percent,
                    i + 1,
                    total,
                    file.FileName
                ));
            }

            _serviceImageRepository.AddRange(serviceImages);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceImageCreatedEvent(request.ServiceId));
            }
        }
    }
}
