using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class MessageRepository : BaseRepository<Message, Guid>, IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Message>> GetAllMessagesBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, Guid conversationId, CancellationToken cancellationToken = default)
        {
            var messages = new List<Message>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getMessages", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@ConversationId", conversationId);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Services)
            while (await reader.ReadAsync(cancellationToken))
            {
                var message = new Message(
                    reader.GetGuid("Id"),
                    reader.GetGuid("SenderId"),
                    reader.IsDBNull("RecipientId") ? null : reader.GetGuid("RecipientId"),
                    reader.GetGuid("ConversationId"),
                    reader.GetString("MessageText"),
                    (MessageType)Enum.Parse(typeof(MessageType), reader.GetString("MessageType"))
                );

                message.SetIsRead(reader.GetBoolean("IsRead"));
                message.SetUnreadCount(reader.GetInt32("UnreadCount"));
                message.SetCreatedAt(reader.GetDateTime("CreatedAt"));
                message.SetLastUpdatedAt(reader.IsDBNull("LastUpdatedAt") ? null : reader.GetDateTime("LastUpdatedAt"));

                messages.Add(message);
            }

            return messages;
        }

        public async Task<IEnumerable<Message>> GetByConversation(Guid conversationId)
        {
            return await DbSet.Where(x => x.ConversationId == conversationId).ToListAsync();
        }
    }
}
