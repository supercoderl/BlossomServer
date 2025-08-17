using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.Files;
using BlossomServer.Domain.Commands.Files.DeleteFile;
using BlossomServer.Domain.Commands.Files.UploadFile;
using BlossomServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IMediatorHandler _bus;

        public FileService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task DeleteExampleFile(string id)
        {
            await _bus.SendCommandAsync(new DeleteFileCommand(id));
        }

        public async Task<string> UploadExampleFile(UploadExampleFileViewModel file)
        {
            return await _bus.QueryAsync(new UploadFileCommand(file.File, null, null, false));
        }
    }
}
