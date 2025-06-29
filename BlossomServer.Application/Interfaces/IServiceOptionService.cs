using BlossomServer.Application.ViewModels.ServiceOptions;

namespace BlossomServer.Application.Interfaces
{
    public interface IServiceOptionService
    {
        public Task<Guid> CreateServiceOptionAsync(CreateServiceOptionViewModel serviceOption);
        public Task UpdateServiceOptionAsync(UpdateServiceOptionViewModel serviceOption);
        public Task DeleteServiceOptionAsync(Guid serviceOptionId);
    }
}
