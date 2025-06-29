using BlossomServer.Application.Interfaces;
using BlossomServer.Application.ViewModels.ServiceOptions;
using BlossomServer.Domain.Commands.ServiceOptions.CreateServiceOption;
using BlossomServer.Domain.Commands.ServiceOptions.DeleteServiceOption;
using BlossomServer.Domain.Commands.ServiceOptions.UpdateServiceOption;
using BlossomServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Services
{
    public class ServiceOptionService : IServiceOptionService
    {
        private readonly IMediatorHandler _bus;

        public ServiceOptionService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<Guid> CreateServiceOptionAsync(CreateServiceOptionViewModel serviceOption)
        {
            var serviceOptionId = Guid.NewGuid();
            await _bus.SendCommandAsync(new CreateServiceOptionCommand(
                serviceOptionId,
                serviceOption.ServiceId,
                serviceOption.VariantName,
                serviceOption.PriceFrom,
                serviceOption.PriceTo,
                serviceOption.DurationMinutes
            ));
            return serviceOptionId;
        }

        public async Task DeleteServiceOptionAsync(Guid serviceOptionId)
        {
            await _bus.SendCommandAsync(new DeleteServiceOptionCommand(serviceOptionId));
        }

        public async Task UpdateServiceOptionAsync(UpdateServiceOptionViewModel serviceOption)
        {
            await _bus.SendCommandAsync(new UpdateServiceOptionCommand(
                serviceOption.ServiceOptionId,
                serviceOption.VariantName,
                serviceOption.PriceFrom,
                serviceOption.PriceTo,
                serviceOption.DurationMinutes
            ));
        }
    }
}
