using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Messages;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IMessageService
    {
        public Task<string> SendPromptAsync(string prompt);
        public Task<Guid> FindConversationIdByParticipantsAsync(Guid recipientId);
        public Task SendMessageAsync(CreateMessageViewModel viewModel);
        public Task<PagedResult<MessageViewModel>> GetAllMessagesAsync(
            PageQuery query,
            bool includeDeleted,
            Guid conversationId,
            string searchTerm = "",
            SortQuery? sortQuery = null);
        public Task DeleteMessage(Guid? messageId, Guid? conversationId);
    }
}
