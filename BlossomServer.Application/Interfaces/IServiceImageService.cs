using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IServiceImageService
    {
        public Task<PagedResult<ServiceImageViewModel>> GetAllServiceImagesAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);

        public Task<Guid> CreateServiceImageAsync(CreateServiceImageViewModel serviceImage);
        public Task<UpdateServiceImageViewModel> UpdateServiceImageAsync(UpdateServiceImageViewModel serviceImage);
    }
}
