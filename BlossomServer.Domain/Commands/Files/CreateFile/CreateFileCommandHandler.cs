using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.FileInfo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Files.CreateFile
{
    public sealed class CreateFileCommandHandler : CommandHandlerBase, IRequestHandler<CreateFileCommand>
    {
        private readonly IFileInfoRepository _fileInfoRepository;

        public CreateFileCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IFileInfoRepository fileInfoRepository
        ) : base(bus, unitOfWork, notifications )
        {
            _fileInfoRepository = fileInfoRepository;
        }

        public async Task Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var fileInfo = new Entities.FileInfo(
                request.FileInfoId,
                request.FileId,
                request.Url,
                request.FileName
            );

            _fileInfoRepository.Add( fileInfo );

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new FileInfoCreatedEvent(fileInfo.Id));
            }
        }
    }
}
