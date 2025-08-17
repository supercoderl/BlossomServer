using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Messages;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Interfaces.Repositories;
using MediatR;

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
            var results = await _messageRepository.GetAllMessagesBySQL(
                            request.SearchTerm,
                            request.IncludeDeleted,
                            request.Query.Page,
                            request.Query.PageSize,
                            request.SortQuery?.Query ?? "Id",
                            "ASC",
                            request.ConversationId,
                            cancellationToken
                        );

            var messages = results.Select(m => MessageViewModel.FromMessage(m, _user.GetUserId())).ToList();

            return new PagedResult<MessageViewModel>(results.Count(), messages, request.Query.Page, request.Query.PageSize);
        }
    }
}
