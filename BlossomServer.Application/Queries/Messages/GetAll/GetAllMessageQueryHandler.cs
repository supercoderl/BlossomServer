using BlossomServer.Application.Extensions;
using BlossomServer.Application.Queries.Payments.GetAll;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Messages;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlossomServer.Application.Queries.Messages.GetAll
{
    public sealed class GetAllMessagesQueryHandler :
                IRequestHandler<GetAllMessagesQuery, PagedResult<MessageViewModel>>
    {
        private readonly ISortingExpressionProvider<MessageViewModel, Message> _sortingExpressionProvider;
        private readonly IMessageRepository _messageRepository;
        private readonly IUser _user;

        public GetAllMessagesQueryHandler(
            IMessageRepository messageRepository,
            IUser user,
            ISortingExpressionProvider<MessageViewModel, Message> sortingExpressionProvider
        )
        {
            _messageRepository = messageRepository;
            _user = user;
            _sortingExpressionProvider = sortingExpressionProvider;
        }

        public async Task<PagedResult<MessageViewModel>> Handle(
            GetAllMessagesQuery request,
            CancellationToken cancellationToken)
        {
            var messagesQuery = _messageRepository
                .GetAllAsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => request.IncludeDeleted || x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {

            }

            var totalCount = await messagesQuery.CountAsync(cancellationToken);

            messagesQuery = messagesQuery.GetOrderedQueryable(request.SortQuery, _sortingExpressionProvider);

            messagesQuery = messagesQuery.Where(m => m.ConversationId == request.ConversationId);

            var messages = await messagesQuery
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .Select(message => MessageViewModel.FromMessage(message, _user.GetUserId()))
                .ToListAsync(cancellationToken);

            return new PagedResult<MessageViewModel>(
                totalCount, messages, request.Query.Page, request.Query.PageSize);
        }
    }
}
