using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using System.Text.Json.Serialization;

namespace BlossomServer.Application.ViewModels.Messages
{
    public sealed class MessageViewModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid? RecipientId { get; set; }
        public Guid ConversationId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public MessageType MessageType { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public int UnreadCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public static MessageViewModel FromMessage(Message message, Guid currentUserId)
        {
            return new MessageViewModel
            {
                Id = message.Id,
                SenderId = message.SenderId,
                RecipientId = message.RecipientId,
                ConversationId = message.ConversationId,
                MessageText = message.MessageText,
                MessageType = message.MessageType,
                Role = GetDetailedMessageRole(message, currentUserId),
                IsRead = message.IsRead,
                UnreadCount = message.UnreadCount,
                CreatedAt = message.CreatedAt,
                LastUpdatedAt = message.LastUpdatedAt
            };
        }

        private static string GetDetailedMessageRole(Message message, Guid currentUserId)
        {
            // Bot sent the message
            if (message.SenderId == Domain.Constants.Ids.Seed.BotId)
            {
                return "assistant";
            }

            // Current user sent the message
            if (message.SenderId == currentUserId)
            {
                return "user"; // or "me", "sender"
            }

            // Another user sent the message
            return "other"; // or "them", "recipient"
        }
    }

    public class GroqResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public List<GroqChoice> Choices { get; set; }

        [JsonPropertyName("usage")]
        public GroqUsage Usage { get; set; }

        [JsonPropertyName("x_groq")]
        public GroqInternalMetadata GroqMetadata { get; set; }

        [JsonPropertyName("service_tier")]
        public string ServiceTier { get; set; }
    }

    public class GroqChoice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public GroqMessage Message { get; set; }

        [JsonPropertyName("reasoning")]
        public string Reasoning { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }

    public class GroqMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class GroqUsage
    {
        [JsonPropertyName("queue_time")]
        public double QueueTime { get; set; }

        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("prompt_time")]
        public double PromptTime { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("completion_time")]
        public double CompletionTime { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }

        [JsonPropertyName("total_time")]
        public double TotalTime { get; set; }
    }

    public class GroqInternalMetadata
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
