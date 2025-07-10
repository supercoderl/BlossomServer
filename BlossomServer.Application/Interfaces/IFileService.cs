using BlossomServer.Application.ViewModels.Files;

namespace BlossomServer.Application.Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadExampleFile(UploadExampleFileViewModel file);
    }
}
