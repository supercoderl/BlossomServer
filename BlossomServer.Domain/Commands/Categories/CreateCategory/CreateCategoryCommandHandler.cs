using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Notifications;
using BlossomServer.Shared.Events.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Categories.CreateCategory
{
    public sealed class CreateCategoryCommandHandler : CommandHandlerBase, IRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            ICategoryRepository categoryRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var category = new Domain.Entities.Category(
                request.CategoryId,
                request.Name,
                request.IsActive,
                request.Priority
            );

            _categoryRepository.Add(category);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new CategoryCreatedEvent(category.Id));
            }
        }
    }
}
