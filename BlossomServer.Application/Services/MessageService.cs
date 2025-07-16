using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Messages.CheckBot;
using BlossomServer.Application.Queries.Messages.FindConversation;
using BlossomServer.Application.Queries.Messages.GetAll;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Messages;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Commands.Messages.CreateMessage;
using BlossomServer.Domain.Commands.Messages.DeleteMessage;
using BlossomServer.Domain.Interfaces;
using BlossomServer.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace BlossomServer.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        private readonly GroqSettings _groqSettings;
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public MessageService(
            IHttpClientFactory httpClientFactory,
            IOptions<GroqSettings> options,
            IMediatorHandler bus,
            IUser user
        )
        {
            _httpClient = httpClientFactory.CreateClient();
            _groqSettings = options.Value;
            _bus = bus;
            _user = user;
        }

        public async Task DeleteMessage(Guid? messageId, Guid? conversationId)
        {
            await _bus.SendCommandAsync(new DeleteMessageCommand(messageId, conversationId));
        }

        public async Task<Guid> FindConversationIdByParticipantsAsync(Guid recipientId)
        {
            return await _bus.QueryAsync(new FindConversationIdQuery(_user.GetUserId(), recipientId));
        }

        public async Task<PagedResult<MessageViewModel>> GetAllMessagesAsync(PageQuery query, bool includeDeleted, Guid conversationId, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllMessagesQuery(query, includeDeleted, conversationId, searchTerm, sortQuery));
        }

        public async Task SendMessageAsync(CreateMessageViewModel viewModel)
        {
            var userMessageId = Guid.NewGuid();
            await _bus.SendCommandAsync(new CreateMessageCommand(
                userMessageId,
                _user.GetUserId(),
                viewModel.RecipientId,
                viewModel.ConversationId,
                viewModel.MessageText,
                viewModel.MessageType
            ));

            // Check if recipient is a bot
            if (await IsBotRecipientAsync(viewModel.RecipientId))
            {
                var botResponse = await SendPromptAsync(viewModel.MessageText);

                if (!string.IsNullOrEmpty(botResponse))
                {
                    var botMessageId = Guid.NewGuid();
                    await _bus.SendCommandAsync(new CreateMessageCommand(
                        botMessageId,
                        viewModel.RecipientId, // Bot ID
                        _user.GetUserId(), // Send back to user
                        viewModel.ConversationId,
                        botResponse,
                        Domain.Enums.MessageType.Text
                    ));
                }
            }
        }

        public async Task<string> SendPromptAsync(string prompt)
        {
            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                model = "compound-beta",
                temperature = 1,
                max_completion_tokens = 1024,
                top_p = 1,
                stream = false
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _groqSettings.BaseUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _groqSettings.ApiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Groq API error: {response.StatusCode}, {error}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GroqResponse>(responseContent);

            var content = result?.Choices?.FirstOrDefault()?.Message?.Content;

            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            return content;
        }

        private async Task<bool> IsBotRecipientAsync(Guid? recipientId)
        {
            if (!recipientId.HasValue) return false;

            var isBot = await _bus.QueryAsync(new CheckBotQuery(recipientId.Value));
            return isBot;
        }
    }
}
