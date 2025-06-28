using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Domain.Errors;
using BlossomServer.Shared.Events.Category;

namespace BlossomServer.Domain.Commands.Categories.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler : CommandHandlerBase, IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            ICategoryRepository categoryRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no category with ID {request.CategoryId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            category.SetName(request.Name);
            category.SetIsActive(request.IsActive);
            category.SetPriority(request.Priority);
            category.SetIcon(request.Icon);
            category.SetUrl(request.Url);
            category.SetUpdatedAt();

            _categoryRepository.Update(category);

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new CategoryUpdatedEvent(request.CategoryId));
            }
        }
    }
}
