using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.ServiceOption;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ServiceOptions.CreateServiceOption
{
    public sealed class CreateServiceOptionCommandHandler : CommandHandlerBase, IRequestHandler<CreateServiceOptionCommand>
    {
        private readonly IServiceOptionRepository _serviceOptionRepository;

        public CreateServiceOptionCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            IServiceOptionRepository serviceOptionRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _serviceOptionRepository = serviceOptionRepository;
        }

        public async Task Handle(CreateServiceOptionCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var serviceOption = new Entities.ServiceOption(
                request.ServiceOptionId,
                request.ServiceId,
                request.VariantName,
                request.PriceFrom,
                request.PriceTo,
                request.DurationMinutes
            );

            _serviceOptionRepository.Add( serviceOption );

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new ServiceOptionCreatedEvent(serviceOption.Id));
            }
        }
    }
}
