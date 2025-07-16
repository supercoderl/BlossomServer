using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.ServiceImages.GetAll;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.ServiceImages.CreateServiceImage;
using BlossomServer.Domain.Commands.ServiceImages.DeleteServiceImage;
using BlossomServer.Domain.Commands.ServiceImages.UpdateServiceImage;
using BlossomServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Services
{
    public class ServiceImageService : IServiceImageService
    {
        private readonly IMediatorHandler _bus;

        public ServiceImageService(
            IMediatorHandler bus    
        )
        {
            _bus = bus;
        }

        public async Task<Guid> CreateServiceImageAsync(CreateServiceImageViewModel serviceImage)
        {
            var imageId = Guid.NewGuid();
            await _bus.SendCommandAsync(new CreateServiceImageCommand(
                imageId,
                serviceImage.ImageFile,
                serviceImage.ServiceId,
                serviceImage.Description
            ));
            return imageId;
        }

        public async Task DeleteServiceImageAsync(List<Guid> serviceImageIds)
        {
            foreach (var id in serviceImageIds)
            {
                await _bus.SendCommandAsync(new DeleteServiceImageCommand(id));
            }
        }

        public async Task<PagedResult<ServiceImageViewModel>> GetAllServiceImagesAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllServiceImagesQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<UpdateServiceImageViewModel> UpdateServiceImageAsync(UpdateServiceImageViewModel serviceImage)
        {
            await _bus.SendCommandAsync(new UpdateServiceImageCommand(serviceImage.ServiceImageId, serviceImage.ImageName, serviceImage.Description));
            return serviceImage;
        }
    }
}
