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

namespace BlossomServer.Domain.Commands.Categories.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler : CommandHandlerBase, IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(
            IMediatorHandler bus,
            IUnitOfWork unitOfWork,
            INotificationHandler<DomainNotification> notifications,
            ICategoryRepository categoryRepository
        ) : base(bus, unitOfWork, notifications)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!await TestValidityAsync(request)) return;

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if(category == null)
            {
                await NotifyAsync(new DomainNotification(
                    request.MessageType,
                    $"There is no category with ID {request.CategoryId}.",
                    ErrorCodes.ObjectNotFound
                ));
                return;
            }

            _categoryRepository.Remove(category);   

            if(await CommitAsync())
            {
                await Bus.RaiseEventAsync(new CategoryDeletedEvent(request.CategoryId));
            }
        }
    }
}
